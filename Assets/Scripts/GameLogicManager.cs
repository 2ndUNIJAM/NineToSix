using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class GameLogicManager : MonoBehaviour
{
    public UISchedule Schedule => schedule;

    public UILectureSpawner LectureSpawner => lectureSpawner;

    public bool IsHoldingLecture => _holdingLectureComponent != null;

    public bool IsOnAction => _isOnAction;

    public int CurrentScore => _currentScore;

    public int CurrentCredit => _currentCredit;

    [SerializeField] private UICurrentStudent currentStudent;
    [SerializeField] private UINextStudent nextStudent1, nextStudent2;
    [SerializeField] private UISchedule schedule;
    [SerializeField] private UILectureSpawner lectureSpawner;
    [SerializeField] private UILectureBucket lectureBucket;
    [SerializeField] private UIScore score;
    [SerializeField] private UITimer timer;
    [SerializeField] private UICredit credit;
    [SerializeField] private UIGameEnd gameEnd;
    [SerializeField] private GameObject actionDimmer, gamePanel;
    [SerializeField] private RankingData rankingData;
    [SerializeField] private ReturnPopup returnPopup;
    [SerializeField] private Button returnButton;

    List<Student> studentList;
    private int _activeStudentIndex;

    private UILecture _holdingLectureComponent;
    private TextAsset _lectureJson;
    private List<LectureData> _lectureData;
    private List<Lecture> _selectedLectures;
    private bool _isOnAction, _isGameEnded;

    private int _currentCredit, _currentScore, _confirmCount;
    private float _currentAbsoluteTime;

    private const int timeLimit = 270;

    private bool isImminent;

    private void Awake()
    {
        _selectedLectures = new List<Lecture>();
    }

    private void Start()
    {
        //LectureData Should Be Loaded First
        LoadLecture();
        LoadStudent();
        VisualizeStudent();

        _currentScore = 0;
        _currentAbsoluteTime = 0;
        score.Initialize();
        returnButton.onClick.AddListener(() => returnPopup.gameObject.SetActive(true));
        
        SoundManager.Instance.PlaySound(EBGMType.InGameBGM);
    }

    private void Update()
    {
        _currentAbsoluteTime += Time.deltaTime;
        timer.UpdateTime(_currentAbsoluteTime);

        if (!_isGameEnded && _currentAbsoluteTime >= timeLimit)
        {
            EndGame();
        }
        else if (_currentAbsoluteTime > 260 && !SoundManager.Instance.IsPlaying(EBGMType.GameOverImminent))
        {
            SoundManager.Instance.PlaySound(EBGMType.GameOverImminent);
        }
    }

    private void OnDestroy()
    {
        SoundManager.Instance.StopSound(EBGMType.InGameBGM);
    }

    private void LoadLecture()
    {
        _lectureJson = Resources.Load<TextAsset>("lectures");
        _lectureData = JsonConvert.DeserializeObject<List<LectureData>>(_lectureJson.text);
    }

    private void LoadStudent()
    {
        var studentJson = Resources.Load<TextAsset>("students");
        var studentNames = JsonConvert.DeserializeObject<List<string>>(studentJson.text);
        studentList = new List<Student>();
        for (int i = 0; i < studentNames.Count; i++)
        {
            studentList.Add(new Student(studentNames[i], i));
        }
        _activeStudentIndex = 0;
        credit.ResetCredit(studentList[_activeStudentIndex].GetStudentMaxCredit());
    }

    private void VisualizeStudent()
    {
        Student curStudent = studentList[_activeStudentIndex];
        curStudent.GetFirstRequirement().OnEnter();
        curStudent.GetSecondRequirement().OnEnter();
        curStudent.GetLastRequirement().OnEnter();

        currentStudent.SetStudent(studentList[_activeStudentIndex]);

        if (_activeStudentIndex + 1 < studentList.Count)
            nextStudent1.SetStudent(studentList[_activeStudentIndex + 1]);
        else
            nextStudent1.gameObject.SetActive(false);

        if (_activeStudentIndex + 2 < studentList.Count)
            nextStudent2.SetStudent(studentList[_activeStudentIndex + 2]);
        else
            nextStudent2.gameObject.SetActive(false);

    }

    public IEnumerable<Lecture> GetSelectedLectures()
    {
        return _selectedLectures;
    }

    public void RemoveSelectedLecture(Lecture lecture)
    {
        _selectedLectures.Remove(lecture);
        AddCredit(-lecture.Credit);
        AddScore(-8);
    }

    public IEnumerable<LectureData> GetLectureData()
    {
        return _lectureData;
    }

    public void TryReserveHoldingLecture()
    {
        var lecture = _holdingLectureComponent.Lecture;

        if (!lectureBucket.IsFull && !lectureBucket.ContainsLecture(lecture))
        {
            lectureBucket.ReserveLecture(lecture);
            _holdingLectureComponent.Remove(false);
            SoundManager.Instance.PlaySound(EBGMType.PutLecture);
        }

        UnholdLecture();
    }

    public void HoldLecture(UILecture lecture)
    {
        _holdingLectureComponent = lecture;
    }

    public void UnholdLecture()
    {
        _holdingLectureComponent = null;
    }

    public void TrashHoldingLecture()
    {
        schedule.HideLecturePreview();
        _holdingLectureComponent.Remove(true);
        UnholdLecture();

        // 장바구니의 강의를 드랍했을 경우 -5 
        AddScore(-5);
    }

    public async UniTaskVoid TrySelectHoldingLecture()
    {
        var lecture = _holdingLectureComponent.Lecture;

        // 동일 강좌를 중복 신청할 수 없습니다.
        foreach (var lec in _selectedLectures)
        {
            if (lec.Data.ID == lecture.Data.ID)
            {
                UnholdLecture();
                return;
            }
        }

        // 스케쥴에 안 맞거나 학점에 안 맞으면 신청할 수 없습니다.
        if (!schedule.IsLectureAvailable(lecture) ||
            _currentCredit + lecture.Credit > studentList[_activeStudentIndex].GetStudentMaxCredit())
        {
            UnholdLecture();
            return;
        }

        _isOnAction = true;
        actionDimmer.SetActive(true);
        var succeed = await schedule.StartLectureKeyAction(lecture);
        if (succeed)
        {
            AddCredit(lecture.Credit);
            _selectedLectures.Add(lecture);
            _holdingLectureComponent.Remove(false);
            currentStudent.CheckRequirements(studentList[_activeStudentIndex]);

            // 과목의 수강신청 확정을 성공했을 경우(wsd같은거 눌러서) +10 
            switch (lecture.Credit)
            {
                case 1:
                    AddScore(2);
                    break;
                case 2:
                    AddScore(3);
                    break;
                case 3:
                    AddScore(5);
                    break;
            }

            // 1 2 3 4 -> 0 1 2 3 -> 1 2 4 8 
            int bitMask = 1 << (studentList[_activeStudentIndex].GetStudentGrade() - 1);
            if (((int)lecture.Data.TargetGrade & bitMask) != 0)
            {
                AddScore(2);
            }

            SoundManager.Instance.PlaySound(EBGMType.PutLecture);
        }
        UnholdLecture();
        actionDimmer.SetActive(false);
        _isOnAction = false;
    }

    public void ConfirmSchedule()
    {
        // 15학점 미만인 경우 예외처리
        if (_currentCredit < 15) return;

        _confirmCount++;

        if (lectureSpawner.GuaranteedLectureExists)
        {
            studentList[_activeStudentIndex].GetFirstRequirement().OnExit();
            studentList[_activeStudentIndex].GetSecondRequirement().OnExit();
            studentList[_activeStudentIndex].GetLastRequirement().OnExit();
        }


        // 선택 요청사항을 만족한 상태로 시간표를 확정했을 경우 (식 활용) 
        int basicScore = 50; // 기본 점수
        float bonusScore = 1.5f; // 보너스 상수
        float reqWeight = studentList[_activeStudentIndex].GetLastRequirement().DoesMeetRequirement() ? 1.15f : 1; // 요구사항 가중치
        int confirmScore = (int)((basicScore + Mathf.Pow(bonusScore, _currentCredit - 15)) * reqWeight);
        AddScore(confirmScore);

        /*Debug.Log($"요구사항 T/F 디버그: {studentList[_activeStudentIndex].GetFirstRequirement().DoesMeetRequirement()} / " +
            $"{studentList[_activeStudentIndex].GetSecondRequirement().DoesMeetRequirement()} / " +
            $"{studentList[_activeStudentIndex].GetLastRequirement().DoesMeetRequirement()} ");*/

        // 학생의 필수 요청사항을 못 지킨 상태로 시간표를 확정했을 경우 -25 
        if (!studentList[_activeStudentIndex].GetFirstRequirement().DoesMeetRequirement()
            || !studentList[_activeStudentIndex].GetSecondRequirement().DoesMeetRequirement())
        {
            AddScore(-25);
            SoundManager.Instance.PlaySound(EBGMType.BadConfirm);
        }
        else
        {
            SoundManager.Instance.PlaySound(EBGMType.GoodConfirm);
        }

        // 조건: activestudentIndex+1, studentList length  초과 시 게임 종료
        if (++_activeStudentIndex >= studentList.Count)
        {
            // 게임 종료
            EndGame();
        }
        else
        {
            // 학생 교체 
            VisualizeStudent();

            // 시간표 클리어 UISchedule.cs에 ClearSlots
            schedule.ClearSlots();
            _selectedLectures.Clear();

            // 수강학점 초기화
            _currentCredit = 0;
            credit.ResetCredit(studentList[_activeStudentIndex].GetStudentMaxCredit());
        }
    }

    public void AddScore(int score)
    {
        _currentScore += score;
        this.score.AddScore(score);
        Debug.Log($"AddScore Called with score: {score}");
    }

    void AddCredit(int credit)
    {
        _currentCredit += credit;
        this.credit.UpdateCredit(_currentCredit);
    }

    private void EndGame()
    {
        _isGameEnded = true;
        SoundManager.Instance.StopSound(EBGMType.GameOverImminent);

        rankingData.AddRanking(_currentScore, _confirmCount);
        gameEnd.gameObject.SetActive(true);
        gameEnd.Init(rankingData);
        gamePanel.SetActive(false);
    }
}

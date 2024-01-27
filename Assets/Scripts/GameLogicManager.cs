using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class GameLogicManager : MonoBehaviour
{
    public UISchedule Schedule => schedule;

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
    [SerializeField] private GameObject actionDimmer;
    [SerializeField] private RankingData rankingData;
    [SerializeField] private GameObject gamePanel;

    List<Student> studentList;
    private int _activeStudentIndex;

    private UILecture _holdingLectureComponent;
    private TextAsset _lectureJson;
    private List<LectureData> _lectureData;
    private List<Lecture> _selectedLectures;
    private bool _isOnAction;

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
    }

    private void Update()
    {
        _currentAbsoluteTime += Time.deltaTime;
        timer.UpdateTime(_currentAbsoluteTime);

        if (_currentAbsoluteTime >= timeLimit)
        {
            EndGame();
        }
        else if (_currentAbsoluteTime > 260 && !SoundManager.Instance.IsPlaying(EBGMType.GameOverImminent))
        {
            SoundManager.Instance.PlaySound(EBGMType.GameOverImminent);
        }
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
            _holdingLectureComponent.Remove();
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
        _holdingLectureComponent.Remove();
        UnholdLecture();

        // 장바구니의 강의를 드랍했을 경우 -5 
        AddScore(-5);
    }

    public async UniTaskVoid TrySelectHoldingLecture()
    {
        var lecture = _holdingLectureComponent.Lecture;

        if (schedule.IsLectureAvailable(lecture) && _currentCredit + lecture.Credit <= studentList[_activeStudentIndex].GetStudentMaxCredit())
        {
            _isOnAction = true;
            actionDimmer.SetActive(true);
            var succeed = await schedule.StartLectureKeyAction(lecture);
            if (succeed)
            {
                AddCredit(lecture.Credit);
                _selectedLectures.Add(lecture);
                _holdingLectureComponent.Remove();
                currentStudent.CheckRequirements(studentList[_activeStudentIndex]);
                // 과목의 수강신청 확정을 성공했을 경우(wsd같은거 눌러서) +10 
                AddScore(10);
                SoundManager.Instance.PlaySound(EBGMType.PutLecture);
            }
            else
            {
                // 과목의 수강신청 확정을 실패했을 경우 -5 
                AddScore(-5);
            }
            UnholdLecture();
            actionDimmer.SetActive(false);
            _isOnAction = false;
        }
    }

    public void ConfirmSchedule() 
    {
        // 제한시간 내에 실시간 수강신청란의 과목을 제거한 경우 +2 > TODO
        // 제한시간 내에 실시간 수강신청란의 과목을 제거하지 못한 경우 -3 > TODO
        // 수강신청 현황란의 강의를 드랍했을 경우 -10 > TODO
        if (_currentCredit < 15) return; // 15학점 미만인 경우 

        _confirmCount++;

        // 선택 요청사항을 만족한 상태로 시간표를 확정했을 경우 (식 활용) 
        int basicScore = 50; // 기본 점수
        int bonusScore = 35; // 보너스 상수
        int bonusWeight = 100;// 보너스 가중치
        float reqWeight = studentList[_activeStudentIndex].GetLastRequirement().DoesMeetRequirement() ? 1.15f : 1; // 요구사항 가중치
        int confirmScore = (int)((basicScore + bonusWeight * (21 - _currentCredit) / (21 - _currentCredit + bonusScore)) * reqWeight);
        AddScore(confirmScore);


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

            // 수강학점 초기화
            _currentCredit = 0;
            credit.ResetCredit(studentList[_activeStudentIndex].GetStudentMaxCredit());
        }
    }

    void AddScore(int score)
    {
        _currentScore += score;
        this.score.UpdateScore(_currentScore);
        Debug.Log($"AddScore Called with score: {score}");
    }

    void AddCredit(int credit)
    {
        _currentCredit += credit;
        this.credit.UpdateCredit(_currentCredit);
    }

    private void EndGame()
    {
        SoundManager.Instance.StopSound(EBGMType.GameOverImminent);

        rankingData.AddRanking(_currentScore, _confirmCount);
        gameEnd.gameObject.SetActive(true);
        gameEnd.Init(rankingData);
        gamePanel.SetActive(false);
    }
}

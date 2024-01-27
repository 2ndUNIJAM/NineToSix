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

    [SerializeField] private UICurrentStudent currentStudent;
    [SerializeField] private UINextStudent nextStudent1, nextStudent2;
    [SerializeField] private UISchedule schedule;
    [SerializeField] private UILectureSpawner lectureSpawner;
    [SerializeField] private UILectureBucket lectureBucket;
    [SerializeField] private GameObject actionDimmer;

    List<Student> studentList;
    private int _activeStudentIndex;

    private UILecture _holdingLectureComponent;
    private TextAsset _lectureJson;
    private List<LectureData> _lectureData;
    private List<Lecture> _selectedLectures;
    private bool _isOnAction;

    private int _currentCredit;
    private int _currentScore;

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

    public bool TryReserveLecture(Lecture lecture)
    {
        if (lectureBucket.IsFull)
            return false;

        lectureBucket.ReserveLecture(lecture);
        return true;
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
        UpdateScore(-5);
    }

    public async UniTaskVoid TrySelectHoldingLecture()
    {
        var lecture = _holdingLectureComponent.Lecture;

        if (schedule.IsLectureAvailable(lecture) && _currentCredit + lecture.Credit <= 18)
        {
            _isOnAction = true;
            actionDimmer.SetActive(true);
            var succeed = await schedule.StartLectureKeyAction(lecture);
            if (succeed)
            {
                _selectedLectures.Add(lecture);
                _currentCredit += lecture.Credit;
                _holdingLectureComponent.Remove();
                currentStudent.CheckRequirements(studentList[_activeStudentIndex]);
                // 과목의 수강신청 확정을 성공했을 경우(wsd같은거 눌러서) +10 
                UpdateScore(10);
            }
            else
            {
                // 과목의 수강신청 확정을 실패했을 경우 -5 
                UpdateScore(-5);
            }
            UnholdLecture();
            actionDimmer.SetActive(false);
            _isOnAction = false;
        }
    }

    public void ConfirmSchedule() // 확정함수
    {
        // 제한시간 내에 실시간 수강신청란의 과목을 제거한 경우 +2 > TODO
        // 제한시간 내에 실시간 수강신청란의 과목을 제거하지 못한 경우 -3 > TODO
        // 수강신청 현황란의 강의를 드랍했을 경우 -10 > TODO


        // 선택 요청사항을 만족한 상태로 시간표를 확정했을 경우 (식 활용) 
        int basicScore = 50; // 기본 점수
        int bonusScore = 35; // 보너스 상수
        int bonusWeight = 100;// 보너스 가중치
        float reqWeight = studentList[_activeStudentIndex].GetLastRequirement().DoesMeetRequirement() ? 1.15f : 1; // 요구사항 가중치
        int confirmScore = (int)((basicScore + bonusWeight * (21 - _currentCredit) / (21 - _currentCredit + bonusScore)) * reqWeight);
        UpdateScore(confirmScore);




        // 학생의 필수 요청사항을 못 지킨 상태로 시간표를 확정했을 경우 -25 
        if (!studentList[_activeStudentIndex].GetFirstRequirement().DoesMeetRequirement()
            || !studentList[_activeStudentIndex].GetSecondRequirement().DoesMeetRequirement())
            UpdateScore(-25);


        // 조건: activestudentIndex+1, studentList length  초과 시 게임 종료
        if(++_activeStudentIndex >= studentList.Count) 
        {
            // 게임 종료
        }
        else
        {
            // 학생 교체 
            VisualizeStudent();

            // 시간표 클리어 UISchedule.cs에 ClearSlots
            schedule.ClearSlots();

            // 수강학점 초기화
            _currentCredit = 0;
        }
    }

    void UpdateScore(int score)
    {
        _currentScore += score;
    }
}

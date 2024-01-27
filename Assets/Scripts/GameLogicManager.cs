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

    public void TrashLecture()
    {
        schedule.HideLecturePreview();
        _holdingLectureComponent.Remove();
        UnholdLecture();
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
            }
            UnholdLecture();
            actionDimmer.SetActive(false);
            _isOnAction = false;
        }
    }

    public void ConfirmSchedule() // 확정함수
    {
        // 0. 점수 계산 



        // 1. 학생 교체 
        ++_activeStudentIndex;
        VisualizeStudent();

        // 2. 시간표 클리어 UISchedule.cs에 ClearSlots
        schedule.ClearSlots();

        // 3. 수강학점 초기화
        _currentCredit = 0;
    }
}

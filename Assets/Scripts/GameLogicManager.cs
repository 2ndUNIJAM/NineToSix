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

    [SerializeField] private UICurrentStudent currentStudent;
    [SerializeField] private UINextStudent nextStudent1, nextStudent2;
    [SerializeField] private UISchedule schedule;
    [SerializeField] private UILectureSpawner lectureSpawner;
    [SerializeField] private UILectureBucket lectureBucket;

    List<Student> studentList;
    private int _activeStudentIndex;

    private UILecture _holdingLectureComponent;
    private GameObject _ghostGraphic;
    private bool _isScheduleDroppable, _isTrashDroppable;
    private TextAsset _lectureJson;
    private List<LectureData> _lectureData;
    private List<Lecture> _selectedLectures;

    private int _currentCredit;

    private void Awake()
    {
        _selectedLectures = new List<Lecture>();
    }

    private void Start()
    {
        _lectureJson = Resources.Load<TextAsset>("lectures");

        // Load lecture
        _lectureData = JsonConvert.DeserializeObject<List<LectureData>>(_lectureJson.text);

        LoadStudent();
        VisualizeStudent();
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

    private void Update()
    {
        if (!IsHoldingLecture || !_ghostGraphic)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            if (_isScheduleDroppable)
                TrySelectHoldingLecture().Forget();

        }
    }

    public IEnumerable<Lecture> GetSelectedLectures()
    {
        return _selectedLectures;
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

    public void SetScheduleDroppable(bool droppable)
    {
        _isScheduleDroppable = droppable;
    }

    public void SetTrashDroppable(bool droppable)
    {
        _isTrashDroppable = droppable;
    }

    public async UniTaskVoid TrySelectHoldingLecture()
    {
        var lecture = _holdingLectureComponent.Lecture;

        if (schedule.IsLectureAvailable(lecture) && _currentCredit + lecture.Credit <= 18)
        {
            var succeed = await schedule.StartLectureKeyAction(lecture);
            if (succeed)
            {
                _selectedLectures.Add(lecture);
                _currentCredit += lecture.Credit;
                _holdingLectureComponent.Remove();
            }
            UnholdLecture();
        }
    }
}

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
    [SerializeField] private UINextStudent nextStudentTemplate;
    [SerializeField] private RectTransform nextStudentParent;
    [SerializeField] private UISchedule schedule;
    [SerializeField] private UILectureSpawner lectureSpawner;
    [SerializeField] private UILectureBucket lectureBucket;

    List<Student> studentList;
    private Student _activeStudent;
    private List<RequirementBase> _requirements;
    private Queue<Student> _nextStudentQueue;

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
    }

    private void LoadStudent()
    {
        var studentJson = Resources.Load<TextAsset>("students");
        var studentNames = JsonConvert.DeserializeObject<List<string>>(studentJson.text);
        for (int i = 0; i < studentNames.Count; i++)
        {
            studentList.Add(new Student(studentNames[i], i));
        }
        _activeStudent = studentList[0];
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

    public void GetCurrentStudent(Student student)
    {
        student = studentList[0];
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

<<<<<<< HEAD
        if (schedule.IsLectureAvailable(lecture) && _currentCredit + lecture.Credit <= 18)
=======
        if (schedule.IsLectureAvailable(lecture) && _currentCredit + lecture.Credit <= _activeStudent.GetStudentMaxCredit())
>>>>>>> 96c5b72b617344c351c703d32fdb2740a8138f5f
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

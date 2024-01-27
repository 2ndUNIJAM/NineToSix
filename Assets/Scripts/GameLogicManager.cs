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

    [SerializeField] private UICurrentStudent currentStudent;
    [SerializeField] private UINextStudent nextStudentTemplate;
    [SerializeField] private RectTransform nextStudentParent;
    [SerializeField] private UISchedule schedule;
    [SerializeField] private UILectureSpawner lectureSpawner;
    [SerializeField] private UILectureBucket lectureBucket;
    [SerializeField] private GameObject actionDimmer;
    
    List<Student> studentList;
    private Student _activeStudent;
    private List<RequirementBase> _requirements;
    private Queue<Student> _nextStudentQueue;
    
    private UILecture _holdingLectureComponent;
    private TextAsset _lectureJson;
    private List<LectureData> _lectureData;
    private List<Lecture> _selectedLectures;
    private bool _isOnAction;

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
}

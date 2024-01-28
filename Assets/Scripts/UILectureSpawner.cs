using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class UILectureSpawner : MonoBehaviour
{
    public bool GuaranteedLectureExists => _guaranteedLectureExists;
    
    public event Action SpawnedLectureVanished;

    public event Action SpawnedLectureProcessed;

    [SerializeField] private GameLogicManager manager;
    [SerializeField] private UISpawnedLecture lectureTemplate;
    [SerializeField] private RectTransform lectureParent;
    
    private List<LectureData> _lectureData;

    private List<LectureData> _majorReqs, _liberalAndMajorBasics;
    private LectureData _guaranteedLecture;
    private bool _guaranteedLectureExists;
    private int _guaranteedCoefficient = 1;

    private const int GuaranteedPercentageStep = 10;

    private void Awake()
    {
        var lectureJson = Resources.Load<TextAsset>("lectures");
        
        // Load lecture source pool
        _lectureData = JsonConvert.DeserializeObject<List<LectureData>>(lectureJson.text);
        
        FillLiberalMajorBasicPool();
        FillMajorRequiredPool();
    }

    private void Start()
    {
        SpawnNewLecture(0);
        SpawnNewLecture(1);
        SpawnNewLecture(2);
    }
    
    // Lecture data manipulation

    public void FillLiberalMajorBasicPool()
    {
        _liberalAndMajorBasics = _lectureData.Where(x => x.Type != ELectureType.MajorRequired).ToList();
    }

    public void FillMajorRequiredPool()
    {
        _majorReqs = _lectureData.Where(x => x.Type == ELectureType.MajorRequired).ToList();

        if (_guaranteedLectureExists)
            SetGuaranteedLecture(_guaranteedLecture.ID);
    }

    public void SetGuaranteedLecture(int id)
    {
        _guaranteedLecture = _lectureData.Find(x => x.ID == id);
        var index = _majorReqs.FindIndex(x => x.ID == _guaranteedLecture.ID);
        if (index >= 0)
            _majorReqs.RemoveAt(index);
        _guaranteedLectureExists = true;
    }

    public void ReturnGuaranteedLectureToPool()
    {
        _majorReqs.Add(_guaranteedLecture);
        _guaranteedLectureExists = false;
    }

    public void SpawnNewLecture(int index)
    {
        var lecture = index == 2 ? CreateMajorRequiredLecture() : CreateLiberalMajorBasicLecture();
        
        var newLecture = Instantiate(lectureTemplate, lectureParent);
        newLecture.Initialize(lecture, index);
        newLecture.Removed += OnSpawnedLectureRemoved;
        newLecture.transform.SetSiblingIndex(index);
        newLecture.transform.localScale = Vector3.one;
        newLecture.gameObject.SetActive(true);
    }

    private Lecture CreateLiberalMajorBasicLecture()
    {
        var randIdx = Random.Range(0, _liberalAndMajorBasics.Count);
        var lecture = new Lecture(_liberalAndMajorBasics[randIdx]);
        _liberalAndMajorBasics.RemoveAt(randIdx);
        
        if(_liberalAndMajorBasics.Count == 0)
            FillLiberalMajorBasicPool();
        return lecture;
    }

    private Lecture CreateMajorRequiredLecture()
    {
        if (_guaranteedLectureExists)
        {
            // Guaranteed Percentage
            var roll = Random.Range(0, 100);
            if (roll < GuaranteedPercentageStep * _guaranteedCoefficient)
            {
                // 확정 출현에 당첨되었습니다.
                _guaranteedCoefficient = 1;
                return new Lecture(_guaranteedLecture);
            }
            
            // 확정 출현에 실패했으므로 등장 확률을 올립니다.
            _guaranteedCoefficient++;
        }
        
        var randIdx = Random.Range(0, _majorReqs.Count);
        var lecture = new Lecture(_majorReqs[randIdx]);
        _majorReqs.RemoveAt(randIdx);
        
        if(_majorReqs.Count == 0)
            FillMajorRequiredPool();
        return lecture;
    }

    private void OnSpawnedLectureRemoved(int index, bool forced, bool vanished)
    {
        if (vanished)
        {
            SpawnedLectureVanished?.Invoke();
            manager.AddScore(-1);
        }
        else if (forced)
        {
            // 점수 변화는 없습니다.
        }
        else
        {
            SpawnedLectureProcessed?.Invoke();
            manager.AddScore(2);
        }

        SpawnNewLecture(index);
    }
}

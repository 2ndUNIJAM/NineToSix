using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class UILectureSpawner : MonoBehaviour
{
    public event Action SpawnedLectureVanished;

    public event Action SpawnedLectureProcessed;

    [SerializeField] private GameLogicManager manager;
    [SerializeField] private UISpawnedLecture lectureTemplate;
    [SerializeField] private RectTransform lectureParent;

    private TextAsset _lectureJson;
    private List<LectureData> _lectureData;

    private void Awake()
    {
        _lectureJson = Resources.Load<TextAsset>("lectures");
        
        // Load lecture
        _lectureData = JsonConvert.DeserializeObject<List<LectureData>>(_lectureJson.text);
    }

    private void Start()
    {
        SpawnNewLecture(0);
        SpawnNewLecture(1);
        SpawnNewLecture(2);
    }

    public void SpawnNewLecture(int index)
    {
        int randIdx = Random.Range(0, _lectureData.Count);
        var lectureObj = new Lecture(_lectureData[randIdx]);
        _lectureData.RemoveAt(randIdx);
        
        var newLecture = Instantiate(lectureTemplate, lectureParent);
        newLecture.Initialize(lectureObj, index);
        newLecture.Removed += OnSpawnedLectureRemoved;
        newLecture.transform.SetSiblingIndex(index);
        newLecture.transform.localScale = Vector3.one;
        newLecture.gameObject.SetActive(true);

        if (_lectureData.Count == 0)
            _lectureData = JsonConvert.DeserializeObject<List<LectureData>>(_lectureJson.text);
    }

    private void OnSpawnedLectureRemoved(int index, bool forced, bool vanished)
    {
        if (vanished)
        {
            SpawnedLectureVanished?.Invoke();
            manager.AddScore(-3);
        }
        else if (forced)
        {
            // 점수 변화는 없습니다.;
        }
        else
        {
            SpawnedLectureProcessed?.Invoke();
            manager.AddScore(2);
        }

        SpawnNewLecture(index);
    }
}

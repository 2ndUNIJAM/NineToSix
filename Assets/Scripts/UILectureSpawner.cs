using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class UILectureSpawner : MonoBehaviour
{
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
        SpawnNewLecture();
        SpawnNewLecture();
        SpawnNewLecture();
    }

    public void SpawnNewLecture()
    {
        int randIdx = Random.Range(0, _lectureData.Count);
        var lectureObj = new Lecture(_lectureData[randIdx]);
        _lectureData.RemoveAt(randIdx);
        
        var newLecture = Instantiate(lectureTemplate, lectureParent);
        newLecture.Initialize(lectureObj);
        newLecture.Removed += OnSpawnedLectureRemoved;
        newLecture.transform.localScale = Vector3.one;
        newLecture.gameObject.SetActive(true);

        if (_lectureData.Count == 0)
            _lectureData = JsonConvert.DeserializeObject<List<LectureData>>(_lectureJson.text);
    }

    private void OnSpawnedLectureRemoved()
    {
        SpawnNewLecture();
    }
}

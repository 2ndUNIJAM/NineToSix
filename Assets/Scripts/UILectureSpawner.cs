using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UILectureSpawner : MonoBehaviour
{
    [SerializeField] private UISpawnedLecture lectureTemplate;
    [SerializeField] private RectTransform lectureParent;

    public void SpawnLecture(Lecture lecture)
    {
        var newLecture = Instantiate(lectureTemplate, lectureParent);
        newLecture.Initialize(lecture);
        newLecture.transform.localScale = Vector3.one;
        newLecture.gameObject.SetActive(true);
    }
}

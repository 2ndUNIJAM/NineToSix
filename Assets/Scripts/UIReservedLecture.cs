using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReservedLecture : MonoBehaviour
{
    public bool IsEmpty => _isEmpty;

    public event Action<bool> Removed;
    
    [SerializeField] private UILecture lectureComponent;

    private bool _isEmpty;

    private void Awake()
    {
        _isEmpty = true;
        lectureComponent.RemoveRequested += RemoveLecture;
    }

    public void SetLecture(Lecture lecture)
    {
        lectureComponent.SetLecture(lecture);
        lectureComponent.gameObject.SetActive(true);
        _isEmpty = false;
    }

    public void RemoveLecture(bool forced)
    {
        Removed?.Invoke(forced);
        lectureComponent.transform.SetParent(transform);
        lectureComponent.transform.localPosition = Vector3.zero;
        lectureComponent.gameObject.SetActive(false);
        _isEmpty = true;
    }

    public bool HasLecture(Lecture lecture) => lecture == lectureComponent.Lecture;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILectureBucket : MonoBehaviour
{
    public bool IsFull => _reservedCount == 4;
    
    [SerializeField] private UIReservedLecture[] reserveSlots;

    private int _reservedCount;

    private void Start()
    {
        foreach (var slot in reserveSlots)
            slot.Removed += OnLectureRemove;
    }

    public void ReserveLecture(Lecture lecture)
    {
        foreach (var slot in reserveSlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetLecture(lecture);
                _reservedCount++;
                return;
            }
        }
    }

    private void OnLectureRemove()
    {
        _reservedCount--;
    }
}

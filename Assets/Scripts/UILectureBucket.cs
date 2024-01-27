using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UILectureBucket : MonoBehaviour, IDropHandler
{
    public bool IsFull => _reservedCount == 4;

    [SerializeField] private GameLogicManager manager;
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

    public bool ContainsLecture(Lecture lecture)
    {
        foreach (var slot in reserveSlots)
        {
            if (!slot.IsEmpty && slot.HasLecture(lecture))
                return true;
        }

        return false;
    }

    private void OnLectureRemove(bool forced)
    {
        _reservedCount--;
        
        if(forced)
        {
            manager.AddScore(-5);
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (manager.IsHoldingLecture)
            manager.TryReserveHoldingLecture();
    }
}

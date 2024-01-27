using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILectureBucket : MonoBehaviour
{
    public bool IsFull => _reservedCount == 4;
    
    [SerializeField] private UIReservedLecture[] reserveSlots;

    private int _reservedCount;

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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISchedule : MonoBehaviour
{
    [SerializeField] private UIScheduleSlot slotTemplate;
    [SerializeField] private RectTransform slotParent;
    
    private UIScheduleSlot[,] _slots;  // [column, row] => [monday, 1]

    private void Awake()
    {
        _slots = new UIScheduleSlot[5, 8];
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var newSlot = Instantiate(slotTemplate, slotParent);
                newSlot.transform.localScale = Vector3.one;
                newSlot.gameObject.SetActive(true);
                _slots[i, j] = newSlot;
            }
        }
    }

    public void ClearSlots()
    {
        foreach (var slot in _slots)
            slot.Clear();
    }

    /// <summary>
    /// 현재 시간표에서 어떤 '시간대'가 비어있는지를 알아봅니다.
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    public bool IsRowClear(int rowIndex)
    {
        var isClear = true;
        for (int i = 0; i < 5; i++)
            isClear = isClear && !_slots[i, rowIndex].Filled;
        return isClear;
    }

    /// <summary>
    /// 현재 시간표에서 어떤 '요일'이 비어있는지를 알아봅니다.
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public bool IsColumnClear(int columnIndex)
    {
        var isClear = true;
        for (int j = 0; j < 8; j++)
            isClear = isClear && !_slots[columnIndex, j].Filled;
        return isClear;
    }

    public void ShowLecturePreview(Lecture lecture)
    {
        
    }

    public bool IsLectureAvailable(Lecture lecture)
    {
        return true;
    }
}

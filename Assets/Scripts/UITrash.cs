using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITrash : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameLogicManager manager;
    
    public void OnDrop(PointerEventData eventData)
    {
        if (manager.IsHoldingLecture)
            manager.TrashHoldingLecture();
    }
}

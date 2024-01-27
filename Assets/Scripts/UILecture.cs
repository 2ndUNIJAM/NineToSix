using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UILecture : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsDragging => _isDragging;

    public Lecture Lecture => _lecture;

    public event Action RemoveRequested;

    [SerializeField] private GameLogicManager manager;
    [SerializeField] private TextMeshProUGUI lectureName;
    [SerializeField] private UIHighlightedText highTextTemplate;
    [SerializeField] private RectTransform highTextParent;
    [SerializeField] private Transform dragParent;

    private bool _isDragging;
    private Transform _originParent;
    private Lecture _lecture;

    public void SetLecture(Lecture lecture)
    {
        _lecture = lecture;
        
        lectureName.text = _lecture.Data.Name;
        // 과목분류
        var typeColor = _lecture.Data.Type switch
        {
            ELectureType.LiberalArt => Color.cyan,
            ELectureType.MajorBasic => Color.green,
            ELectureType.MajorRequired => Color.magenta,
            _ => Color.black
        };
        var typeText = _lecture.Data.Type switch
        {
            ELectureType.LiberalArt => "교양",
            ELectureType.MajorBasic => "전기",
            ELectureType.MajorRequired => "전필",
            _ => "일선"
        };
        CreateHighlightedText(typeColor, typeText);
        // 학년
        if((int)_lecture.Data.TargetGrade == 0b1111)
            CreateHighlightedText(Color.yellow, "전 학년");
        else
        {
            if ((_lecture.Data.TargetGrade & ELectureGrade.First) != 0)
                CreateHighlightedText(Color.yellow, "1학년");
            if ((_lecture.Data.TargetGrade & ELectureGrade.Second) != 0)
                CreateHighlightedText(Color.yellow, "2학년");
            if ((_lecture.Data.TargetGrade & ELectureGrade.Third) != 0)
                CreateHighlightedText(Color.yellow, "3학년");
            if ((_lecture.Data.TargetGrade & ELectureGrade.Fourth) != 0)
                CreateHighlightedText(Color.yellow, "4학년");
        }
    }

    private void CreateHighlightedText(Color color, string text)
    {
        var highText = Instantiate(highTextTemplate, highTextParent);
        highText.Initialize(color, text);
        highText.transform.localScale = Vector3.one;
        highText.gameObject.SetActive(true);
    }

    public void Remove()
    {
        RemoveRequested?.Invoke();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.Schedule.ShowLecturePreview(_lecture);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.Schedule.HideLecturePreview();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        manager.HoldLecture(this);
        _originParent = transform.parent;
        transform.SetParent(dragParent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_originParent);
        transform.localPosition = Vector3.zero;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        manager.UnholdLecture();
    }
}

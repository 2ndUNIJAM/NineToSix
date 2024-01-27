using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UILecture : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsDragging => _isDragging;

    public bool IsKeyActionTarget => manager.Schedule.ActioningLecture == _lecture;

    public bool IsBeingRemoved => _isBeingRemoved;

    public Lecture Lecture => _lecture;

    public event Action RemoveRequested;

    [SerializeField] private GameLogicManager manager;
    [SerializeField] private TextMeshProUGUI lectureName;
    [SerializeField] private UIHighlightedText highTextTemplate;
    [SerializeField] private RectTransform highTextParent;
    [SerializeField] private UIStarGauge starGauge;
    [SerializeField] private Transform dragParent;
    [SerializeField] private bool preventRightClick = false;

    private bool _isDragging, _isBeingRemoved;
    private Transform _originParent;
    private Lecture _lecture;
    private List<GameObject> _highTexts = new List<GameObject>();

    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 1);
        transform.DOScale(Vector3.one, 0.2f);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void OnDestroy()
    {
        if (manager.Schedule.PreviewingLecture == _lecture)
            manager.Schedule.HideLecturePreview();
    }

    public void SetLecture(Lecture lecture)
    {
        // 초기화
        _lecture = lecture;
        _isDragging = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        lectureName.text = _lecture.Data.Name;
        // 과목분류
        foreach (var obj in _highTexts)
            Destroy(obj);
        _highTexts.Clear();
        
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
        // 별점
        starGauge.SetValue(_lecture.Rating);
    }

    private void CreateHighlightedText(Color color, string text)
    {
        var highText = Instantiate(highTextTemplate, highTextParent);
        highText.Initialize(color, text);
        highText.transform.localScale = Vector3.one;
        highText.gameObject.SetActive(true);
        _highTexts.Add(highText.gameObject);
    }

    public void Remove()
    {
        _isBeingRemoved = true;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        DOTween.Sequence()
            .Append(transform.DOScale(new Vector3(0, 0, 1), 0.2f))
            .AppendCallback(() =>
            {
                RemoveRequested?.Invoke();
            }).Play();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (manager.IsHoldingLecture)
            return;
        
        manager.Schedule.ShowLecturePreview(_lecture);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (manager.IsHoldingLecture)
            return;
        
        if (!_isDragging)
            manager.Schedule.HideLecturePreview();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Started dragging.");
        _isDragging = true;
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
        _isDragging = false;

        if (!manager.IsOnAction)
        {
            manager.Schedule.HideLecturePreview();
            manager.UnholdLecture();
        }
        Debug.Log("Ended dragging.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Remove();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScheduleTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lectureName;
    [SerializeField] private UIHighlightedText highTextTemplate;
    [SerializeField] private RectTransform highTextParent;
    [SerializeField] private UIStarGauge starGauge;
    [SerializeField] private Canvas canvas;
    
    private bool _visible;
    private List<GameObject> _highTexts;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        _rectTransform.anchoredPosition = (Vector2)(Input.mousePosition / canvas.scaleFactor) + new Vector2(20, 0);
    }

    public void Show(Lecture lecture)
    {
        lectureName.text = lecture.Data.Name;
        // 과목분류
        if (_highTexts == null)
            _highTexts = new List<GameObject>();
        foreach (var obj in _highTexts)
            Destroy(obj);
        _highTexts.Clear();
        
        Color typeColor = lecture.Data.Type switch
        {
            ELectureType.LiberalArt => new Color32(168, 202, 115, 255),
            ELectureType.MajorBasic => new Color32(125, 166, 232, 255),
            ELectureType.MajorRequired => new Color32(240, 134, 118, 255),
            _ => Color.black
        };
        var typeText = lecture.Data.Type switch
        {
            ELectureType.LiberalArt => "교양",
            ELectureType.MajorBasic => "전기",
            ELectureType.MajorRequired => "전필",
            _ => "일선"
        };
        CreateHighlightedText(typeColor, typeText);
        // 학년
        if((int)lecture.Data.TargetGrade == 0b1111)
            CreateHighlightedText(Color.yellow, "전 학년");
        else
        {
            if ((lecture.Data.TargetGrade & ELectureGrade.First) != 0)
                CreateHighlightedText(Color.yellow, "1학년");
            if ((lecture.Data.TargetGrade & ELectureGrade.Second) != 0)
                CreateHighlightedText(Color.yellow, "2학년");
            if ((lecture.Data.TargetGrade & ELectureGrade.Third) != 0)
                CreateHighlightedText(Color.yellow, "3학년");
            if ((lecture.Data.TargetGrade & ELectureGrade.Fourth) != 0)
                CreateHighlightedText(Color.yellow, "4학년");
        }
        // 별점
        starGauge.SetValue(lecture.Rating);
        
        void CreateHighlightedText(Color color, string text)
        {
            var highText = Instantiate(highTextTemplate, highTextParent);
            highText.Initialize(color, text);
            highText.transform.localScale = Vector3.one;
            highText.gameObject.SetActive(true);
            _highTexts.Add(highText.gameObject);
        }

        gameObject.SetActive(true);
        _visible = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _visible = false;
    }
}

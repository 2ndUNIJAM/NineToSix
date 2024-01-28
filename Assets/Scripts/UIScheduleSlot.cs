using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

public class UIScheduleSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool Filled => _filled;

    public event Action<Lecture> RemoveLectureRequested;
    
    [SerializeField] private Graphic colorGraphic;
    [SerializeField] private Image radialSlider;
    [SerializeField] private TextMeshProUGUI keyActionText;
    [SerializeField] private UIScheduleTooltip tooltip;

    private float _keyActionTimeLimit, _keyActionElapsedTime;
    private bool _filled, _isOnKeyAction;
    private Color _fillColor = Color.white;
    private Lecture _fillingLecture;

    private void Update()
    {
        if (!_isOnKeyAction)
            return;

        _keyActionElapsedTime += Time.deltaTime;
        radialSlider.fillAmount = 1 - _keyActionElapsedTime / _keyActionTimeLimit;
    }

    public void StartPreview(Color color)
    {
        if (_filled)
            colorGraphic.color = Color.grey;
        else
            colorGraphic.color = new Color(color.r, color.g, color.b, 0.5f);
    }

    public void StopPreview()
    {
        colorGraphic.color = _fillColor;
    }

    public void ShowKeyAction(Color color, KeyCode key, float timeLimit)
    {
        _isOnKeyAction = true;
        _keyActionTimeLimit = timeLimit;
        _keyActionElapsedTime = 0;
        
        colorGraphic.color = color;
        _fillColor = color;
        radialSlider.fillAmount = 1;
        radialSlider.gameObject.SetActive(true);
        keyActionText.text = key.ToString();
        keyActionText.gameObject.SetActive(true);
    }

    public void CompleteKeyAction()
    {
        _isOnKeyAction = false;
        radialSlider.DOFillAmount(1, 0.3f);
    }

    public void ShowFailedKeyAction()
    {
        _isOnKeyAction = false;
        radialSlider.DOFillAmount(1, 0.3f);
        keyActionText.text = "X";
    }

    public void Confirm(Lecture lecture)
    {
        _filled = true;
        _fillingLecture = lecture;
        _isOnKeyAction = false;
        radialSlider.gameObject.SetActive(false);
        keyActionText.gameObject.SetActive(false);
    }

    public void Clear()
    {
        _filled = false;
        _fillColor = Color.white;
        _isOnKeyAction = false;
        radialSlider.gameObject.SetActive(false);
        keyActionText.gameObject.SetActive(false);
        colorGraphic.color = _fillColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_filled)
            return;
        if (eventData.pointerDrag != null)
            return;
        
        tooltip.Show(_fillingLecture);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_filled)
            return;
        if (eventData.pointerDrag != null)
            return;

        tooltip.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_filled)
            return;
        if (eventData.button != PointerEventData.InputButton.Right)
            return;
        
        // TODO: 팝업창 통해 의사 물어보기
        RemoveLectureRequested?.Invoke(_fillingLecture);
        tooltip.Hide();
    }
}

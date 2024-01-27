using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;

public class UIScheduleSlot : MonoBehaviour
{
    public bool Filled => _filled;
    
    [SerializeField] private Graphic colorGraphic;
    [SerializeField] private Image radialSlider;
    [SerializeField] private TextMeshProUGUI keyActionText;

    private float _keyActionTimeLimit, _keyActionElapsedTime;
    private bool _filled, _isOnKeyAction;
    private Color _fillColor = Color.white;
    private Tween _tween;

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
            _tween = DOTween.Sequence()
                .Append(colorGraphic.DOColor(Color.black, 0.3f).SetEase(Ease.InCubic))
                .Append(colorGraphic.DOColor(_fillColor, 0.3f).SetEase(Ease.OutCubic))
                .SetLoops(-1).Play();
        else
            colorGraphic.color = new Color(color.r, color.g, color.b, 0.5f);
    }

    public void StopPreview()
    {
        if (_tween != null && _tween.active)
            _tween.Kill();
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

    public void Confirm()
    {
        _filled = true;
        _isOnKeyAction = false;
        radialSlider.gameObject.SetActive(false);
        keyActionText.gameObject.SetActive(false);
    }

    public void Clear()
    {
        if (_tween != null && _tween.active)
            _tween.Kill();
        
        _filled = false;
        _fillColor = Color.white;
        _isOnKeyAction = false;
        radialSlider.gameObject.SetActive(false);
        keyActionText.gameObject.SetActive(false);
        colorGraphic.color = _fillColor;
    }
}

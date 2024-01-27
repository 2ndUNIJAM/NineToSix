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
    private Tween _tween;

    private void Update()
    {
        if (!_isOnKeyAction)
            return;

        _keyActionElapsedTime += Time.deltaTime;
        radialSlider.fillAmount = _keyActionElapsedTime / _keyActionTimeLimit;
    }

    public void StartPreview(Color color)
    {
        _tween = DOTween.Sequence()
            .Append(colorGraphic.DOColor(color, 0.3f).SetEase(Ease.InCubic))
            .Append(colorGraphic.DOColor(Color.white, 0.3f).SetEase(Ease.OutCubic))
            .SetLoops(-1).Play();
    }

    public void ShowKeyAction(Color color, KeyCode key, float timeLimit)
    {
        _isOnKeyAction = true;
        _keyActionTimeLimit = timeLimit;
        _keyActionElapsedTime = 0;
        
        colorGraphic.color = color;
        radialSlider.fillAmount = 1;
        radialSlider.gameObject.SetActive(true);
        keyActionText.text = key.ToString();
        keyActionText.gameObject.SetActive(true);
    }

    public void CompleteKeyAction()
    {
        _isOnKeyAction = true;
        radialSlider.fillAmount = 1;
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
        if (_tween.active)
            _tween.Kill();
        _filled = false;
        _isOnKeyAction = false;
        radialSlider.gameObject.SetActive(false);
        keyActionText.gameObject.SetActive(false);
        colorGraphic.color = Color.white;
        colorGraphic.gameObject.SetActive(false);
    }
}

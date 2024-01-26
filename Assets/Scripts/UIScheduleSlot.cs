using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIScheduleSlot : MonoBehaviour
{
    public bool Filled => _filled;
    
    [SerializeField] private Image colorImage;
    [SerializeField] private TextMeshProUGUI keyActionText;

    private bool _filled;
    private Tween _tween;

    public void StartPreview(Color color)
    {
        _tween = DOTween.Sequence()
            .Append(colorImage.DOColor(color, 0.3f).SetEase(Ease.InCubic))
            .Append(colorImage.DOColor(Color.white, 0.3f).SetEase(Ease.OutCubic))
            .SetLoops(-1).Play();
    }

    public void ShowKeyAction(Color color, KeyCode key)
    {
        colorImage.color = color;
        keyActionText.text = key.ToString();
        keyActionText.gameObject.SetActive(true);
    }

    public void Confirm()
    {
        _filled = true;
        keyActionText.gameObject.SetActive(false);
    }

    public void Clear()
    {
        if (_tween.active)
            _tween.Kill();
        _filled = false;
        colorImage.color = Color.white;
        colorImage.gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UISpawnedLecture : MonoBehaviour
{
    public event Action<int, bool> Removed;
    
    [SerializeField] private UILecture lectureComponent;
    [SerializeField] private Slider timerSlider;
    // TODO: 별 슬라이더 추가

    private int _index;
    private int _timeLimit;
    private float _elapsedTime;
    private UILecture _ghostGraphic;
    private RectTransform _rectTransform;

    private bool _isVanishing;

    private void Update()
    {
        if (lectureComponent.IsDragging || lectureComponent.IsKeyActionTarget)
            return;
        
        if (_elapsedTime < _timeLimit)
        {
            _elapsedTime += Time.deltaTime;
            timerSlider.value = _timeLimit - _elapsedTime;

            if (_elapsedTime >= _timeLimit)
            {
                _isVanishing = true;
                lectureComponent.Remove();
            }
        }
    }

    public void Initialize(Lecture lecture, int index)
    {
        _index = index;
        _timeLimit = lecture.TimeLimit;
        timerSlider.maxValue = _timeLimit;
        timerSlider.value = _timeLimit;
        _elapsedTime = 0;

        lectureComponent.SetLecture(lecture);
        lectureComponent.RemoveRequested += Remove;
    }

    private void Remove()
    {
        Removed?.Invoke(_index, !_isVanishing);
        
        if (lectureComponent && lectureComponent.transform.parent != transform)
            Destroy(lectureComponent.gameObject);
        Destroy(gameObject);
    }
}

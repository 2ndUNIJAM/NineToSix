using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UISpawnedLecture : MonoBehaviour
{
    public event Action Removed;
    
    [SerializeField] private UILecture lectureComponent;
    [SerializeField] private Slider timerSlider;
    // TODO: 별 슬라이더 추가

    private int _timeLimit;
    private float _elapsedTime;
    private UILecture _ghostGraphic;
    private RectTransform _rectTransform;

    private void Update()
    {
        if (_elapsedTime < _timeLimit)
        {
            _elapsedTime += Time.deltaTime;
            timerSlider.value = _timeLimit - _elapsedTime;

            if (_elapsedTime >= _timeLimit)
                Remove();
        }
    }

    public void Initialize(Lecture lecture)
    {
        _elapsedTime = 0;

        lectureComponent.SetLecture(lecture);
        lectureComponent.RemoveRequested += Remove;
    }

    public void Remove()
    {
        Removed?.Invoke();
        
        if (lectureComponent.transform.parent != transform)
            Destroy(lectureComponent.gameObject);
        Destroy(gameObject);
    }
}

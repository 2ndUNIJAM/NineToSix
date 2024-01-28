using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen _instance;

    [SerializeField] private GameObject panel;
    
    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        GameManager.Instance.SceneChangeStarted += OnSceneChangeStart;
        // GameManager.Instance.SceneChangeProgress += OnSceneChangeProgress;
        GameManager.Instance.SceneChangeFinished += OnSceneChangeFinish;
    }

    private void OnSceneChangeStart(float delay)
    {
        panel.SetActive(true);
    }

    private void OnSceneChangeProgress(float progress)
    {
        // TODO: 만약 로딩 프로그레스를 표시하고 싶으면 여기서 진행합니다.
    }

    private void OnSceneChangeFinish()
    {
        panel.SetActive(false);
    }
}

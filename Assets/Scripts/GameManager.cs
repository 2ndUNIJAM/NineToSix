using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                var singleton = new GameObject("GameManager", typeof(GameManager));
                _instance = singleton.GetComponent<GameManager>();
                DontDestroyOnLoad(singleton);
            }

            return _instance;
        }
    }

    private static GameManager _instance;

    /// <summary>
    /// 씬 전환이 요청되었을 때 호출됩니다. 실제 씬 전환 시작까지의 딜레이(초 단위)가 매개변수로 전달됩니다.
    /// </summary>
    public event Action<float> SceneChangeStarted;

    /// <summary>
    /// 씬 전환이 진행 중일 때 매 프레임마다 호출됩니다. 씬 전환 진행도(0~1 사이)가 매개변수로 전달됩니다.
    /// </summary>
    public event Action<float> SceneChangeProgress;

    /// <summary>
    /// 씬 전환이 완료되었을 때 호출됩니다.
    /// </summary>
    public event Action SceneChangeFinished;

    private string _currentScene;
    private Stack<string> _sceneHistory;

    private const float SceneChangeDelay = 0f;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _sceneHistory = new Stack<string>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 지정된 씬으로 전환합니다. 이전 씬이 씬 기록에 추가됩니다.
    /// </summary>
    /// <param name="nextScene"></param>
    public void ChangeToScene(string nextScene)
    {
        _sceneHistory.Push(_currentScene);
        _currentScene = nextScene;
        ChangeSceneAsync(_currentScene).Forget();
    }

    /// <summary>
    /// 씬 기록을 참고하여 직전 씬으로 전환합니다.
    /// </summary>
    public void BackToPreviousScene()
    {
        if (_sceneHistory.Count == 0)
            return;

        _currentScene = _sceneHistory.Pop();
        ChangeSceneAsync(_currentScene).Forget();
    }

    private async UniTaskVoid ChangeSceneAsync(string nextScene)
    {
        SceneChangeStarted?.Invoke(SceneChangeDelay);
        await UniTask.WaitForSeconds(SceneChangeDelay);

        var op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            SceneChangeProgress?.Invoke(Mathf.InverseLerp(0, 0.9f, op.progress));
            if (op.progress >= 0.9f)
                op.allowSceneActivation = true;
            await UniTask.NextFrame();
        }

        SceneChangeFinished?.Invoke();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

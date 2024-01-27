using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EBGMType // Resources/Sound 폴더 내 파일과 일치해야 함
{
    ClickButton,
    DragLecture,
    RemoveLecture,
    GoodConfirm,
    BadConfirm,
    GameOverImminent
}

public class SoundManager : MonoBehaviour
{
    AudioClip[] soundEffects;
    GameObject[] speakers;

    public static SoundManager Instance
    {
        get
        {
            if (!_instance)
            {
                var singleton = new GameObject("SoundManager", typeof(SoundManager));
                _instance = singleton.GetComponent<SoundManager>();
                DontDestroyOnLoad(singleton);
            }

            return _instance;
        }
    }
    private static SoundManager _instance;

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

        soundEffects = Resources.LoadAll<AudioClip>("Sound/");

        speakers = new GameObject[soundEffects.Length];
        for(int i=0; i<soundEffects.Length; ++i)
        {
            speakers[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Speaker"));
        }

        for (int i = 0; i < speakers.Length; ++i)
        {
            speakers[i].GetComponent<AudioSource>().clip = soundEffects[i];
            Debug.Log(speakers[i].GetComponent<AudioSource>().clip);
        }
    }
    
    public void PlaySound(EBGMType bgm)
    {
        speakers[(int)bgm].GetComponent<AudioSource>().Play();
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EBGMType // Resources/Sound 폴더 내 파일과 일치해야 함
{
    ClickButton,
    PutLecture,
    RemoveLecture,
    GoodConfirm,
    BadConfirm,
    GameOverImminent,
    InGameBGM
    // 시작화면 bgm 추가 필요
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
        }
    }
    
    public void PlaySound(EBGMType bgm)
    {
        if(bgm == EBGMType.GameOverImminent || bgm == EBGMType.InGameBGM)
        {
            // bgm loop 설정
            speakers[(int)bgm].GetComponent<AudioSource>().loop = true;
        }
        speakers[(int)bgm].GetComponent<AudioSource>().Play();
    }

    public void StopSound(EBGMType bgm)
    {
        speakers[(int)bgm].GetComponent<AudioSource>().Stop();
    }

    public bool IsPlaying(EBGMType bgm)
    {
        if (speakers[(int)bgm].GetComponent<AudioSource>().isPlaying)
        {
            return true;
        }
        return false;
    }
}

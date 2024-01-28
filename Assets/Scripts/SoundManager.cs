using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EBGMType // Resources/Sound 폴더 내 파일과 일치해야 함
{
    ClickButton, // 0
    PutLecture,
    RemoveLecture,
    GoodConfirm,
    BadConfirm,
    GameOverImminent, // 5
    SerialHit,
    SerialHit2,
    SerialHit3,
    SerialHitFail,
    StartMenuBGM, // 10
    InGameBGM, 
    EndGameAlarm // 12
}

public class SoundManager : MonoBehaviour
{
    AudioClip[] soundEffects;
    GameObject[] speakers;
    int serialHitCount;
    int maxSerialHitCount;

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
            speakers[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Speaker"), transform);
        }

        for (int i = 0; i < speakers.Length; ++i)
        {
            speakers[i].GetComponent<AudioSource>().clip = soundEffects[i];
        }


        speakers[(int)EBGMType.StartMenuBGM].GetComponent<AudioSource>().loop = true;
        speakers[(int)EBGMType.StartMenuBGM].GetComponent<AudioSource>().volume = 0.5f;

        speakers[(int)EBGMType.InGameBGM].GetComponent<AudioSource>().loop = true;
        speakers[(int)EBGMType.InGameBGM].GetComponent<AudioSource>().volume = 0.5f;

        speakers[(int)EBGMType.GameOverImminent].GetComponent<AudioSource>().loop = true;

        serialHitCount = 0;
    }

    public void PlaySound(EBGMType bgm)
    {
/*        if(bgm == EBGMType.StartMenuBGM) 
        {
            if(IsPlaying(EBGMType.InGameBGM))
            {
                StopSound(EBGMType.InGameBGM);
            }
        }
        else if(bgm == EBGMType.InGameBGM)
        {
            if (IsPlaying(EBGMType.StartMenuBGM))
            {
                StopSound(EBGMType.StartMenuBGM);
            }
        }*/
        if(bgm == EBGMType.SerialHit)
        {
            // 1 3: 2 / 2 3: 1 / 3 3: 0 || 1 2 : 1 / 2 2 : 0 || 1 1 : 0 
            switch (maxSerialHitCount - ++serialHitCount)
            {
                case 2:
                    speakers[(int)EBGMType.SerialHit].GetComponent<AudioSource>().Play();
                    break;
                case 1:
                    speakers[(int)EBGMType.SerialHit2].GetComponent<AudioSource>().Play();
                    break;
                case 0: // 최대값 도달
                    speakers[(int)EBGMType.SerialHit3].GetComponent<AudioSource>().Play();
                    ResetSerialHitCount();
                    break;
            }
            return;
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

    public void ResetSerialHitCount()
    {
        serialHitCount = 0;
    }

    public void SetMaxSerialHitCount(int num)
    {
        maxSerialHitCount = num;
    }
}

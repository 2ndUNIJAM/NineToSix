using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class UISchedule : MonoBehaviour, IDropHandler
{
    public Lecture PreviewingLecture => _previewingLecture;

    public Lecture ActioningLecture => _actioningLecture;
    
    [SerializeField] private GameLogicManager manager;
    [SerializeField] private UIScheduleSlot slotTemplate;
    [SerializeField] private RectTransform slotParent;
    
    private UIScheduleSlot[,] _slots;  // [column, row] => [monday, 1]
    private Lecture _previewingLecture, _actioningLecture;

    private void Awake()
    {
        _slots = new UIScheduleSlot[5, 8];
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var newSlot = Instantiate(slotTemplate, slotParent);
                newSlot.RemoveLectureRequested += OnRemoveLectureRequested;
                newSlot.transform.localScale = Vector3.one;
                newSlot.gameObject.SetActive(true);
                _slots[i, j] = newSlot;
            }
        }
    }

    public void ClearSlots()
    {
        foreach (var slot in _slots)
            slot.Clear();
    }

    /// <summary>
    /// 현재 시간표에서 어떤 '시간대'가 비어있는지를 알아봅니다.
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    public bool IsRowClear(int rowIndex)
    {
        var isClear = true;
        for (int i = 0; i < 5; i++)
            isClear = isClear && !_slots[i, rowIndex].Filled;
        return isClear;
    }

    /// <summary>
    /// 현재 시간표에서 어떤 '요일'이 비어있는지를 알아봅니다.
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public bool IsColumnClear(int columnIndex)
    {
        var isClear = true;
        for (int j = 0; j < 8; j++)
            isClear = isClear && !_slots[columnIndex, j].Filled;
        return isClear;
    }

    /// <summary>
    /// 주어진 강의를 시간표에서 프리뷰합니다.
    /// </summary>
    /// <param name="lecture"></param>
    public void ShowLecturePreview(Lecture lecture)
    {
        _previewingLecture = lecture;
        Color previewColor = _previewingLecture.Data.Type switch
        {
            ELectureType.LiberalArt => new Color32(168, 202, 115, 255),
            ELectureType.MajorBasic => new Color32(125, 166, 232, 255),
            ELectureType.MajorRequired => new Color32(240, 134, 118, 255),
            _ => Color.black
        };

        foreach (var schedulePos in _previewingLecture.Schedule)
        {
            var slotFilled = _slots[schedulePos.x, schedulePos.y].Filled;
            _slots[schedulePos.x, schedulePos.y].StartPreview(slotFilled ? Color.black : previewColor);
        }
    }

    /// <summary>
    /// 현재 프리뷰 중인 강의의 프리뷰를 종료합니다.
    /// </summary>
    public void HideLecturePreview()
    {
        if (_previewingLecture == null)
            return;
        
        foreach (var schedulePos in _previewingLecture.Schedule)
            _slots[schedulePos.x, schedulePos.y].StopPreview();
        _previewingLecture = null;
    }

    /// <summary>
    /// 주어진 강의가 시간표에서 자리가 있는지에 대해 알아봅니다.
    /// </summary>
    /// <param name="lecture"></param>
    /// <returns></returns>
    public bool IsLectureAvailable(Lecture lecture)
    {
        var isEmpty = true;
        foreach (var schedulePos in lecture.Schedule)
            isEmpty = isEmpty && !_slots[schedulePos.x, schedulePos.y].Filled;
        return isEmpty;
    }

    public async UniTask<bool> StartLectureKeyAction(Lecture lecture)
    {
        _actioningLecture = lecture;
        
        var keyCodes = new List<KeyCode> { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
        var typeColor = lecture.Data.Type switch
        {
            ELectureType.LiberalArt => new Color32(168, 202, 115, 255),
            ELectureType.MajorBasic => new Color32(125, 166, 232, 255),
            ELectureType.MajorRequired => new Color32(240, 134, 118, 255)
        };
        var keyActionSlots = new Dictionary<KeyCode, UIScheduleSlot>();
        var elapsedTime = 0f;
        
        InitializeKeyAction();

        SoundManager.Instance.SetMaxSerialHitCount(lecture.Credit);
        while (elapsedTime < 3f)
        {
            if (KeyboardInputExists())
            {
                var currentKey = KeyCode.None;
                foreach (var keyCode in keyActionSlots.Keys)
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        currentKey = keyCode;
                        break;
                    }
                }

                if (currentKey != KeyCode.None)
                {
                    keyActionSlots[currentKey].CompleteKeyAction();
                    keyActionSlots.Remove(currentKey);

                    SoundManager.Instance.PlaySound(EBGMType.SerialHit);
                }
                else
                {
                    foreach (var schedulePos in lecture.Schedule)
                        _slots[schedulePos.x, schedulePos.y].ShowFailedKeyAction();
                    
                    SoundManager.Instance.PlaySound(EBGMType.SerialHitFail); // 딜레이 느림. 수정 필요 
                    SoundManager.Instance.ResetSerialHitCount();
                    await UniTask.WaitForSeconds(0.5f);

                    InitializeKeyAction();
                }
            }

            if (keyActionSlots.Count == 0)
            {
                await UniTask.WaitForSeconds(0.5f);
                foreach (var schedulePos in lecture.Schedule)
                    _slots[schedulePos.x, schedulePos.y].Confirm(_actioningLecture);
                _actioningLecture = null;
                return true;
            }

            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        foreach (var schedulePos in lecture.Schedule)
            _slots[schedulePos.x, schedulePos.y].Clear();
        _actioningLecture = null;
        return false;
        
        void InitializeKeyAction()
        {
            keyCodes = new List<KeyCode> { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
            keyActionSlots.Clear();
            foreach (var schedulePos in lecture.Schedule)
            {
                var randIdx = Random.Range(0, keyCodes.Count);
                _slots[schedulePos.x, schedulePos.y].ShowKeyAction(typeColor, keyCodes[randIdx], 3f);
                keyActionSlots.Add(keyCodes[randIdx], _slots[schedulePos.x, schedulePos.y]);
                keyCodes.RemoveAt(randIdx);
            }
            keyCodes = new List<KeyCode> { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
            elapsedTime = 0;
        }

        bool KeyboardInputExists()
        {
            return Time.timeScale >= 0.5f &&
                   Input.anyKeyDown &&
                   !Input.GetMouseButtonDown(0) &&
                   !Input.GetMouseButtonDown(1) &&
                   !Input.GetMouseButtonDown(2);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped.");
        if (manager.IsHoldingLecture)
        {
            // 즉, 그것이 아직 프리뷰 중임을 의미합니다.
            HideLecturePreview();
            manager.TrySelectHoldingLecture().Forget();
        }
    }

    public void OnRemoveLectureRequested(Lecture lecture)
    {
        foreach (var schedulePos in lecture.Schedule)
            _slots[schedulePos.x, schedulePos.y].Clear();
        manager.RemoveSelectedLecture(lecture);
    }
}

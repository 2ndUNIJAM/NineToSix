using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIConfirmButton : MonoBehaviour
{
    Button button;
    [SerializeField] GameLogicManager gLogicManager;
    Image image;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(gLogicManager.ConfirmSchedule);

        image = GetComponent<Image>();
    }
    void Update()
    {
        if (gLogicManager.CurrentCredit < 15)
            image.color = Color.gray;
        else
            image.color = Color.yellow;
    }


}

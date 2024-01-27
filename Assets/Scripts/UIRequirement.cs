using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRequirement : MonoBehaviour
{
    // TODO: Requirement 객체 가지기
    [SerializeField] private Image backImage;
    [SerializeField] private TextMeshProUGUI textLabel;
    [SerializeField] private GameObject completedCheck;

    public void SetText(string explain)
    {
        textLabel.text = explain;
    }
}

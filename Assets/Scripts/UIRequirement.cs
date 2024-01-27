using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRequirement : MonoBehaviour
{
    [SerializeField] private Sprite check;
    [SerializeField] private TextMeshProUGUI textLabel;
    [SerializeField] private Image checkBox;

    public void SetText(string explain)
    {
        textLabel.text = explain;
    }

    public void CheckBox(bool satisfy)
    {
        if (satisfy)
        {
            checkBox.sprite = check;
            checkBox.type = Image.Type.Simple;
        }
        else
            checkBox.sprite = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHighlightedText : MonoBehaviour
{
    [SerializeField] private Graphic graphic;
    [SerializeField] private TextMeshProUGUI textLabel;

    public void Initialize(Color color, string text)
    {
        graphic.color = color;
        textLabel.text = text;
    }
}

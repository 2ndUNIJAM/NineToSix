using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStarGauge : MonoBehaviour
{
    [SerializeField] private GameObject[] emptyStar;
    [SerializeField] private GameObject[] fullStar;
    [SerializeField] private TextMeshProUGUI valueText;

    private int _value;

    public void SetValue(int value)
    {
        _value = value;
        valueText.text = _value.ToString();

        for (int i = 0; i < 5; i++)
        {
            emptyStar[i].SetActive(i >= _value);
            fullStar[i].SetActive(i < _value);
        }
    }
}

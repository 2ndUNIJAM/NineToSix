using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    [SerializeField] private TMP_Text textTime;

    public void Initialize()
    {
        textTime.text = "9:00";
    }

    public void UpdateTime(float absoluteTime)
    {
        int convertedTime = (int)Math.Round(absoluteTime * 2, 0);
        int convertedMinute = (convertedTime) % 60;
        int convertedHour = (convertedTime) / 60 + 9;

        if (convertedHour >= 18)
            textTime.text = "18:00";
        else
            textTime.text = $"{convertedHour:D2}:{convertedMinute:D2}";
    }
}

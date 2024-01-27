using UnityEngine;
using TMPro;

public class RankingPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
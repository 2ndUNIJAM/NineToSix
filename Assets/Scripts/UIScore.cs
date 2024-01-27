using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;

    public void Initialize()
    {
        textScore.text = "SCORE : 0";
    }

    public void UpdateScore(int score)
    {
        textScore.text = $"SCORE : {score}";
    }
}

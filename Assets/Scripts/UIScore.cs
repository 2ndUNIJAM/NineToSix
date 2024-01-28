using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private UIScoreUpdate updatePrefab;
    [SerializeField] private Transform updateHolder;
    int currentScore = 0;

    public void Initialize()
    {
        textScore.text = "SCORE : 0";
    }

    public void AddScore(int score)
    {
        currentScore += score;
        textScore.text = $"SCORE : {currentScore}";
        var scoreUpdate = Instantiate(updatePrefab, updateHolder);
        scoreUpdate.Init(score);
    }


}

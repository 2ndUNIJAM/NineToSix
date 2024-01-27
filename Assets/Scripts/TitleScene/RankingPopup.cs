using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingPopup : Popup
{
    [SerializeField] private TMP_Text first;
    [SerializeField] private RankingPrefab rankingPrefab;
    [SerializeField] private RankingData rankingData;
    [SerializeField] private Transform prefabHolder;

    protected override void StartAction()
    {
        base.StartAction();
        DisplayRanking();
    }

    private void DisplayRanking()
    {
        int grade = 1;
        foreach (var ranking in rankingData.GetDescendingRanking())
        {
            if (grade == 1)
                first.text = $"1위 - {ranking.PlayerName}, {ranking.PlayerScore}점";
            else
            {
                var textObject = Instantiate(rankingPrefab, prefabHolder);
                textObject.SetText($"{grade}위 - {ranking.PlayerName}, {ranking.PlayerScore}점");
            }
            grade++;
        }
    }
}
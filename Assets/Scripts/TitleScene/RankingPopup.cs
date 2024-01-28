using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class RankingPopup : Popup
{
    [SerializeField] private RankingData rankingData;
    [SerializeField] private RankingPrefab first, rankingPrefab;
    [SerializeField] private Transform rankingPagePrefab, rankingPageHolder, firstPagePrefabHolder;
    [SerializeField] private Button left, right;
    [SerializeField] private List<Transform> rankingPageList;

    private int currentPage;

    protected override void StartAction()
    {
        base.StartAction();
        left.onClick.AddListener(() => ChangeRankingPage(-1));
        right.onClick.AddListener(() => ChangeRankingPage(1));

        currentPage = 0;
        GenerateRanking();
        ChangeRankingPage(0);
    }

    private void GenerateRanking()
    {
        rankingData.Load();
        var sortedRanking = rankingData.GetDescendingRanking();
        first.gameObject.SetActive(sortedRanking.Count() > 0);

        int grade = 1;
        foreach (var ranking in sortedRanking)
        {
            if (grade == 1)
                first.SetText($"1위 - {ranking.PlayerName}, {ranking.PlayerScore}점");
            else if (grade <= 10)
            {
                var textObject = Instantiate(rankingPrefab, firstPagePrefabHolder);
                textObject.SetText($"{grade}위 - {ranking.PlayerName}, {ranking.PlayerScore}점");
            }
            else
            {
                var pageIndex = grade > 10 ? ((grade - 11) / 12) + 1 : 0;
                while (pageIndex >= rankingPageList.Count)
                {
                    rankingPageList.Add(Instantiate(rankingPagePrefab, rankingPageHolder));
                    rankingPageList.Last().gameObject.SetActive(false);
                }

                var textObject = Instantiate(rankingPrefab, rankingPageList[pageIndex]);
                textObject.SetText($"{grade}위 - {ranking.PlayerName}, {ranking.PlayerScore}점");
            }
            grade++;
        }
    }

    private void ChangeRankingPage(int diff)
    {
        if (currentPage + diff >= rankingPageList.Count || currentPage + diff < 0)
            return;

        rankingPageList[currentPage].gameObject.SetActive(false);
        currentPage += diff;
        rankingPageList[currentPage].gameObject.SetActive(true);

        left.gameObject.SetActive(currentPage > 0);
        right.gameObject.SetActive(currentPage + 1 < rankingPageList.Count);
    }
}
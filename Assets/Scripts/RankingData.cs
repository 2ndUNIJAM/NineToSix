using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string PlayerName;
    public int PlayerScore;
    public int ConfirmCount;
    public PlayerData(string name, int score, int confirm)
    {
        PlayerName = name;
        PlayerScore = score;
        ConfirmCount = confirm;
    }
}

[CustomEditor(typeof(RankingData))]
public class RankingEditor : Editor
{
    private RankingData rankingData;
    private void OnEnable()
    {
        rankingData = target as RankingData;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Clear Ranking"))
            rankingData.Clear();
    }
}

[CreateAssetMenu(fileName = "RankingData", menuName = "Data/RankingData")]
public class RankingData : ScriptableObject
{

    [SerializeField] private string playerName;
    [SerializeField] private List<PlayerData> ranking;


    public void SetPlayerName(string name)
    {
        this.playerName = name;
    }

    public void AddRanking(int score, int count)
    {
        ranking.Add(new PlayerData(this.playerName, score, count));
    }

    public IEnumerable<PlayerData> GetDescendingRanking()
    {
        return from playerData in ranking orderby playerData.PlayerScore descending select playerData;
    }

    public void Clear()
    {
        ranking.Clear();
    }

    public bool InTop10()
    {
        var lastData = ranking.Last();
        var scores = from playerData in ranking orderby playerData.PlayerScore descending select playerData.PlayerScore;
        return scores.Count() <= 10 || lastData.PlayerScore >= scores.ElementAt(9);
    }

    public PlayerData GetLast()
    {
        return ranking.Last();
    }
}
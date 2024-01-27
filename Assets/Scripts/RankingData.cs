using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Ranking
{
    public string PlayerName;
    public int PlayerScore;
    public Ranking(string name, int score)
    {
        PlayerName = name;
        PlayerScore = score;
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
    [SerializeField] private List<Ranking> rankingData;


    public void SetPlayerName(string name)
    {
        this.playerName = name;
    }

    public void AddRanking(int score)
    {
        rankingData.Add(new Ranking(this.playerName, score));
    }

    public IEnumerable<Ranking> GetDescendingRanking()
    {
        return from ranking in rankingData orderby ranking.PlayerScore descending select ranking;
    }

    public void Clear()
    {
        rankingData.Clear();
    }
}
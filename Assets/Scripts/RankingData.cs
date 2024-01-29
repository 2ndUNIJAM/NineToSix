using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using System.IO;

[System.Serializable]
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

#if UNITY_EDITOR
[CustomEditor(typeof(RankingData))]
public class RankingEditor : Editor
{
    private RankingData rankingData;
    private int generateAmount;

    private void OnEnable()
    {
        rankingData = target as RankingData;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Clear Ranking"))
            rankingData.Clear();
        if (GUILayout.Button("Generate Random Player Data"))
            GenerateRandomPlayer(generateAmount);
        generateAmount = EditorGUILayout.IntField("Generate Amount", generateAmount);
    }

    private void GenerateRandomPlayer(int amount = 1)
    {
        for (int iter = 0; iter < amount; iter++)
        {
            var nameLength = Random.Range(4, 9);
            var sb = new StringBuilder();
            for (int i = 0; i < nameLength; i++)
            {
                sb.Append((char)Random.Range((int)'a', (int)'z' + 1));
            }
            rankingData.AddRanking(sb.ToString(), Random.Range(0, 1001), Random.Range(0, 11));
        }
    }
}
#endif

[CreateAssetMenu(fileName = "RankingData", menuName = "Data/RankingData")]
public class RankingData : ScriptableObject
{

    [SerializeField] private string playerName;
    [SerializeField] private List<PlayerData> ranking;
    private const string saveKey = "RANKING";

    public void SetPlayerName(string name)
    {
        this.playerName = name;
    }

    public void AddRanking(int score, int count)
    {
        ranking.Add(new PlayerData(this.playerName, score, count));
    }

    public void AddRanking(string name, int score, int count)
    {
        ranking.Add(new PlayerData(name, score, count));
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

    public void Save()
    {
        var saveData = JsonConvert.SerializeObject(ranking);
        PlayerPrefs.SetString(saveKey, saveData);
    }

    public void TryLoad()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            var saveData = PlayerPrefs.GetString(saveKey);
            ranking = JsonConvert.DeserializeObject<List<PlayerData>>(saveData);
        }
        else
            ranking = new List<PlayerData>();
    }
}
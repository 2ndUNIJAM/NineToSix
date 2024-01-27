using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameEnd : MonoBehaviour
{
    [SerializeField] private TMP_Text nickname, score, confirmCount;
    [SerializeField] private GameObject top10;
    [SerializeField] private Button retry, home;

    public void Init(RankingData rankingData)
    {
        retry.onClick.AddListener(() => GameManager.Instance.ChangeToScene("GameScene"));
        home.onClick.AddListener(() => GameManager.Instance.ChangeToScene("TitleScene"));

        var playerData = rankingData.GetLast();
        nickname.text = playerData.PlayerName;
        score.text = playerData.PlayerScore.ToString();
        confirmCount.text = $"확정한 시간표 수 : {playerData.ConfirmCount}개";
        top10.SetActive(rankingData.InTop10());
    }
}
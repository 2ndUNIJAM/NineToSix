using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;


public class TitleSceneManager : MonoBehaviour
{
    [Serializable]
    private class PopupButton
    {
        public Button Button;
        public Popup Popup;
        public PopupButton(Button button, Popup popup)
        {
            this.Button = button;
            this.Popup = popup;
        }
    }

    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private List<PopupButton> popupButtonList;
    [SerializeField] private RankingData rankingData;
    private string nickname;
    private const string gameScene = "GameScene";

    private void Start()
    {
        foreach (var popupButton in popupButtonList)
        {
            popupButton.Button.onClick.AddListener(() => ActivatePopup(popupButton.Popup));
        }

        nicknameInput.onValueChanged.AddListener(UpdateNickname);
        startButton.onClick.AddListener(TryStartGame);
        
        SoundManager.Instance.PlaySound(EBGMType.StartMenuBGM);
    }

    private void OnDestroy()
    {
        SoundManager.Instance.StopSound(EBGMType.StartMenuBGM);
    }

    private void ActivatePopup(Popup popup)
    {
        popup.gameObject.SetActive(true);
    }

    private void UpdateNickname(string input)
    {
        nickname = input;
    }

    private void TryStartGame()
    {
        if (nickname == null || nickname.Length == 0 || nickname.Length > 10)
            return;

        rankingData.SetPlayerName(nickname);
        GameManager.Instance.ChangeToScene(gameScene);
    }
}
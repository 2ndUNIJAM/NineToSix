using UnityEngine;
using UnityEngine.UI;

public class ReturnPopup : Popup
{
    [SerializeField] private Button returnButton;

    protected override void StartAction()
    {
        base.StartAction();
        returnButton.onClick.AddListener(() => GameManager.Instance.ChangeToScene("TitleScene"));
        returnButton.onClick.AddListener(() => this.gameObject.SetActive(false));
    }

    protected override void OnEnableAction()
    {
        base.OnEnableAction();
        Time.timeScale = 0;
    }

    protected override void OnDisableAction()
    {
        base.OnEnableAction();
        Time.timeScale = 1;
    }
}
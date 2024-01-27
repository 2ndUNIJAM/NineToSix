using UnityEngine;
using UnityEngine.UI;

public class ExitPopup : Popup
{
    [SerializeField] private Button quit;

    protected override void StartAction()
    {
        base.StartAction();
        quit.onClick.AddListener(GameManager.Instance.Quit);
    }
}
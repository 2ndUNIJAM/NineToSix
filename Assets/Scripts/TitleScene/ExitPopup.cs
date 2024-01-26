using UnityEngine;
using UnityEngine.UI;

public class ExitPopup : Popup
{
    [SerializeField] private Button quit;

    protected override void StartAction()
    {
        base.StartAction();
        quit.onClick.AddListener(Quit);
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
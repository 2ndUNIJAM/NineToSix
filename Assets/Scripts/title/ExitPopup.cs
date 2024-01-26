using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ExitPopup : MonoBehaviour
{
    [SerializeField] private Button yes, no;

    private void Start()
    {
        yes.onClick.AddListener(Quit);
        no.onClick.AddListener(Cancel);
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Cancel()
    {
        this.gameObject.SetActive(false);
    }
}
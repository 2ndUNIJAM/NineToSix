using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_InputField nickNameInput;
    [SerializeField] private ExitPopup exitPopup;

    private void Start()
    {
        exitButton.onClick.AddListener(ActivateExitPopup);
    }

    private void ActivateExitPopup()
    {
        exitPopup.gameObject.SetActive(true);
    }
}
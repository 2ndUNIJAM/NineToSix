using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Button exit;

    private void Start()
    {
        StartAction();
    }

    protected virtual void StartAction()
    {
        exit.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        this.gameObject.SetActive(false);
    }
}
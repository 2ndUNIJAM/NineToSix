using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Button exit;

    private void Start()
    {
        StartAction();
    }

    private void OnEnable()
    {
        OnEnableAction();
    }

    private void OnDisable()
    {
        OnDisableAction();
    }

    protected virtual void StartAction()
    {
        exit.onClick.AddListener(Exit);
    }

    protected virtual void OnEnableAction() { }

    protected virtual void OnDisableAction() { }

    private void Exit()
    {
        this.gameObject.SetActive(false);
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICredit : MonoBehaviour
{
    [SerializeField] private TMP_Text textCredit;
    int maxCredit = 0;

    public void ResetCredit(int maxCredit)
    {
        this.maxCredit = maxCredit;
        textCredit.text = $"0 / {this.maxCredit}";
    }

    public void UpdateCredit(int currentCredit)
    {
        textCredit.text = $"{currentCredit} / {this.maxCredit}";
    }
}

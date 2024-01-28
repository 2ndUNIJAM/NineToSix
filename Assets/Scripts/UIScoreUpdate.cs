using UnityEngine;
using TMPro;

public class UIScoreUpdate : MonoBehaviour
{
    [SerializeField] private TMP_Text textUpdate;
    [SerializeField] private int movingSpeed;
    public void Init(int update)
    {
        textUpdate.text = update > 0 ? $"+{update}" : update.ToString();
        textUpdate.color = update > 0 ? Color.green : Color.red;
    }

    private void Update()
    {
        this.transform.position += Vector3.up * movingSpeed * Time.deltaTime;
        if (this.transform.position.y > 550)
            Destroy(this.gameObject);
    }
}
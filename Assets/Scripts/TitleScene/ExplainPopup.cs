using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainPopup : Popup
{
    [SerializeField] private Button left, right;
    [SerializeField] private List<GameObject> imageList;
    private int currentIndex;
    protected override void StartAction()
    {
        base.StartAction();
        left.onClick.AddListener(() => ChangeImage(-1));
        right.onClick.AddListener(() => ChangeImage(1));
        currentIndex = 0;
        ChangeImage(0);
    }

    private void ChangeImage(int diff)
    {
        if (currentIndex + diff >= imageList.Count || currentIndex + diff < 0)
            return;

        imageList[currentIndex].SetActive(false);
        currentIndex += diff;
        imageList[currentIndex].SetActive(true);

        left.gameObject.SetActive(currentIndex > 0);
        right.gameObject.SetActive(currentIndex + 1 < imageList.Count);
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainPopup : Popup
{
    [SerializeField] private Button left, right;
    [SerializeField] private List<GameObject> imageList;
    private GameObject currentImage;
    private int currentIndex = 0;
    protected override void StartAction()
    {
        base.StartAction();
        left.onClick.AddListener(() => ShowImage(--currentIndex));
        right.onClick.AddListener(() => ShowImage(++currentIndex));
        ShowImage(0);
    }

    private void ShowImage(int index)
    {
        if (currentIndex >= imageList.Count || currentIndex < 0)
            return;

        if (currentImage != null)
            currentImage.SetActive(false);
        imageList[index].SetActive(true);
        currentImage = imageList[index];
    }
}
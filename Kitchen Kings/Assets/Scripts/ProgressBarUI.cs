using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;

    private void Start()
    {
        barImage.fillAmount = 0;
        Hide();
    }
    public void progress_Fun(float currentProgress)
    {
        barImage.fillAmount = currentProgress;
    }

    public void Hide()
    {
        barImage.fillAmount = 0;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;


    private void Start()
    {
        Hide();

        KitchenGameManager.Instance.OnStateChange += KitchenGameManger_OnStateChange;
    }

    private void KitchenGameManger_OnStateChange(object sender, System.EventArgs e)
    {
        if(KitchenGameManager.Instance.IsCountdown() == true)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        countdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountdownTimer()).ToString();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] Image clockImage;

    private void Start()
    {
        Hide();

        KitchenGameManager.Instance.OnStateChange += KitchenGameManger_OnStateChange;
    }

    private void KitchenGameManger_OnStateChange(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsPlaying() == true)
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
        clockImage.fillAmount = KitchenGameManager.Instance.GetPlayingTimer() / KitchenGameManager.Instance.GetPlayingTimerMax();
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

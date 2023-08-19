using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardUI : MonoBehaviour
{
    [SerializeField] private Button checkLeaderBoard_Button;

    [SerializeField] private Button resumeButton;

    private void Awake()
    {
        checkLeaderBoard_Button.onClick.AddListener(() =>
        {
            Show();
            Time.timeScale = 0;  // pause the game

            PlayFabManager.Instance.GetLeaderBoard();
        });

        resumeButton.onClick.AddListener(() =>
        {
            Hide();
            Time.timeScale = 1f;  // resume the game
        });
    }
    private void Start()
    {
        Hide();

        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;
    }

    private void KitchenGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if(KitchenGameManager.Instance.IsGameOver() == true)
        {
            PlayFabManager.Instance.GetLeaderBoard();     // 
        }
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

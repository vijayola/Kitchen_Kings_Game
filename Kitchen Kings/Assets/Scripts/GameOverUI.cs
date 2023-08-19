using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberOfRecipesDeliveredText;

    [SerializeField] private DeliveryManager deliveryManager;

    [SerializeField] private Button replayButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        replayButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.SetState_to_Countdown();
        });

        exitButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenuScene");
        });
    }

    private void Start()
    {
        Hide();

        KitchenGameManager.Instance.OnStateChange += KitchenGameManger_OnStateChange;
    }

    private void KitchenGameManger_OnStateChange(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver() == true)
        {
            Show();

            int recipes_delivered = deliveryManager.RecipesDelivered();
            numberOfRecipesDeliveredText.text = recipes_delivered.ToString();

            PlayFabManager.Instance.SendLeaderBoard(recipes_delivered);   // update the backend leaderboard
        }
        else
        {
            Hide();
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

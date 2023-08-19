using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button exitButton;   // to go to main menu

    [SerializeField] GameObject optionsUI;

    private void Start()
    {
        Hide();

        resumeButton.onClick.AddListener(Resume);
        optionsButton.onClick.AddListener(Options);
        exitButton.onClick.AddListener(Exit);

        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Debug.Log("paused UI visible !!");
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Resume()
    {
        Time.timeScale = 1;
        Hide();

        KitchenGameManager.Instance.isPaused = false;
    }

    private void Options()
    {
        optionsUI.SetActive(true);
    }

    private void Exit()
    {
        Loader.Load("MainMenuScene");
    }
}

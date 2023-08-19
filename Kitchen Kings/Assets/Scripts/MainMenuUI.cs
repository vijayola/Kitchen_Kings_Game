using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button creditsButton;

    [SerializeField] GameObject usernameUI;

    [SerializeField] GameObject creditsUI;

    private string username;

    private void Awake()
    {
        playButton.onClick.AddListener(Play);
        quitButton.onClick.AddListener(Quit);
        creditsButton.onClick.AddListener(Credits);
    }

    private void Play()
    {

        username = PlayerPrefs.GetString("UserName", "");

        if(username.Length == 0)             // check if the player gas given a username
        {
            usernameUI.SetActive(true);  // Show()
        }
        else
        {
            //SceneManager.LoadScene("Kitchen_Scene");  call this using Loader class

            Loader.Load("Kitchen_Scene");

            Time.timeScale = 1.0f;
        }

    }

    private void Quit()
    {
        Application.Quit();
    }
        
    private void Credits()
    {
        creditsUI.SetActive(true);
    }
}

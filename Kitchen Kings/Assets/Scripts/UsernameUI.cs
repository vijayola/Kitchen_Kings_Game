using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsernameUI : MonoBehaviour
{
    [SerializeField] private InputField nameInput;

    [SerializeField] private Button checkNameButton;

    [SerializeField] private TextMeshProUGUI errorText;

    private void Start()
    {
        Hide();

        checkNameButton.onClick.AddListener(CheckName);
    }

    private void CheckName()
    {
        string input = nameInput.text;
        if(input.Length == 0)
        {
            errorText.text = "Enter Name !!";
        }else if(input.Length > 15) {
            errorText.text = "Name Too Long !!";
        }
        else
        {
            PlayerPrefs.SetString("UserName" , input);
            PlayerPrefs.Save();

            Hide();


            Loader.Load("Kitchen_Scene");  // Load the Scene

            Time.timeScale = 1.0f;
        }
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

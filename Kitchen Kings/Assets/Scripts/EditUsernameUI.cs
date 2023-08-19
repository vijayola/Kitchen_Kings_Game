using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditUsernameUI : MonoBehaviour
{
    [SerializeField] private InputField nameInput;

    [SerializeField] private Button checkNameButton;

    [SerializeField] private TextMeshProUGUI errorText;

    public event EventHandler OnUsernameChange;

    private void Start()
    {
        Hide();

        checkNameButton.onClick.AddListener(CheckName);

        nameInput.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("UserName", "");   // edit placeholder

        nameInput.text = PlayerPrefs.GetString("UserName", "");
    }

    private void CheckName()
    {
        string input = nameInput.text;
        if (input.Length == 0)
        {
            errorText.text = "Enter Name !!";
        }
        else if (input.Length > 15)
        {
            errorText.text = "Name Too Long !!";
        }
        else
        {
            PlayerPrefs.SetString("UserName", input);
            PlayerPrefs.Save();

            Hide();

            OnUsernameChange?.Invoke(this, EventArgs.Empty);
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

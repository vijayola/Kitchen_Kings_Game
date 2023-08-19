using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{
    [SerializeField] Button background_as_Button;

    [SerializeField] TextMeshProUGUI nameText;

    [SerializeField] GameObject editUsernameUI;

    private void Start()
    {
        nameText.text = PlayerPrefs.GetString("UserName");

        background_as_Button.onClick.AddListener(EditUsernameUI);

        editUsernameUI.GetComponent<EditUsernameUI>().OnUsernameChange += ProfileUI_OnUsernameChange;
    }

    private void ProfileUI_OnUsernameChange(object sender, System.EventArgs e)
    {
        nameText.text = PlayerPrefs.GetString("UserName");
    }

    private void EditUsernameUI()
    {
        editUsernameUI.SetActive(true);
    }
}

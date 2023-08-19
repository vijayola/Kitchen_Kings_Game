using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Rendering;
using TMPro;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance;


    [SerializeField] GameObject rowPrefab;
    [SerializeField] GameObject rowParent;

    public void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Login();
    }

    // Login

    private void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
        
    }

    private void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful Login");
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error in Login");
        Debug.Log(error.GenerateErrorReport());
    }

    // LeaderBoard

    public void SendLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Recipes_Cooked",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    private void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("LeaderBoard updated Successfully");
    }

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Recipes_Cooked",
            StartPosition = 0,
            MaxResultsCount = 7
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }

    private void OnLeaderBoardGet(GetLeaderboardResult result)
    {
        foreach(Transform child in rowParent.transform)
        {
            Destroy(child);
        }

        foreach(var item in result.Leaderboard)
        {
            GameObject new_row = Instantiate(rowPrefab, rowParent.transform);
            new_row.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (item.Position + 1).ToString();
            new_row.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("UserName");
            new_row.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (item.StatValue).ToString();

            Debug.Log("position : " + item.Position + " , playerID : " + item.PlayFabId + ", Score : " + item.StatValue);
        }
    }

}

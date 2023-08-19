using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private ClearCounter myCounter;
    [SerializeField] private GameObject[] visualsArray;

    private ClearCounter selectedCounter;

    private void Start()
    {
        player.OnCounterChange += Player_OnCounterChange;       
    }

    private void Player_OnCounterChange(object sender, System.EventArgs e)
    {
        selectedCounter = player.SelectedCounterClass();
        if (selectedCounter == myCounter)
        {
            foreach (GameObject visual in visualsArray)
            {
                visual.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject visual in visualsArray)
            {
                visual.SetActive(false);
            }
        }
    }
}

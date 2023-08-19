using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenGameManager : MonoBehaviour
{
    [SerializeField] Button pauseButton;   // for pausing the game
    public bool isPaused;

    public event EventHandler OnGamePaused;


    public static KitchenGameManager Instance;

    public event EventHandler OnStateChange;
    private enum States
    {
        WaitingToStart,
        Countdown,
        Playing,
        GameOver
    }

    private States state;

    private float waitingToStartTimer = 0f;
    private float countdownTimer = 3f;

    [SerializeField] private float playingTimerMax = 20f;
    private float playingTimer = 0f;
    
    private void Awake()
    {
        Instance = this;
        state = States.WaitingToStart;
    }


    private void Start()
    {
        isPaused = false;
        pauseButton.onClick.AddListener(Pause);
    }

    private void Update()
    {
        switch(state)
        {
            case States.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if(waitingToStartTimer < 0f)
                {
                    state = States.Countdown;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;

            case States.Countdown:
                countdownTimer -= Time.deltaTime;
                if (countdownTimer < 0f)
                {
                    state = States.Playing;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;

            case States.Playing:
                playingTimer += Time.deltaTime;
                if (playingTimer >= playingTimerMax)
                {
                    state = States.GameOver;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;

            case States.GameOver:
                OnStateChange?.Invoke(this, EventArgs.Empty);
                break;
        }
        Debug.Log(state);
    }
    
    public bool IsCountdown()
    {
        return state == States.Countdown;
    }
    public bool IsPlaying()
    {
        return state == States.Playing;
    }

    public bool IsGameOver()
    {
        return state == States.GameOver;
    }


    public float GetCountdownTimer()
    {
        return countdownTimer; 
    }

    public float GetPlayingTimer()
    {
        return playingTimer;
    }

    public float GetPlayingTimerMax()
    {
        return playingTimerMax;
    }

    public void SetState_to_Countdown()
    {
        state = States.Countdown;
    }


    private void Pause()
    {
        if(isPaused == false)
        {
            Time.timeScale = 0f;
            isPaused = true;

            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingSound : MonoBehaviour
{
    [SerializeField] private AudioClipsSO audioClipsSO;
    private AudioSource audioSource;
    private PlayerController playerController;

    private float footstepTimer = 0;
    private float footstepTimerMax = 0.2f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();

    }
    private void Update()
    {
        Debug.Log("player walking :" + playerController.IsWalking());
        if(playerController.IsWalking() == false)
        {
            return;
        }
        footstepTimer += Time.deltaTime;
        if(footstepTimer > footstepTimerMax)
        {
            footstepTimer = 0;
            audioSource.clip = audioClipsSO.footsteps[Random.Range(0, audioClipsSO.footsteps.Length - 1)];
            audioSource.volume = SoundManager.instance.GetVolume();
            audioSource.Play();
        }
    }
}

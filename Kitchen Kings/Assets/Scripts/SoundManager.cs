using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClipsSO audioClipsSO;

    private float volume = 1f;

    [SerializeField] private Slider slider;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        DeliveryManager.instance.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;
        DeliveryManager.instance.OnDeliveryFailure += DeliveryManager_OnDeliveryFailure;
        PlayerController.instance.OnPickup += Player_OnPickup;
        PlayerController.instance.OnDrop += Player_OnDrop;
        PlayerController.instance.OnTrashed += Player_OnTrashed;

        slider.onValueChanged.AddListener((v) =>
        {
            volume = slider.value;
        });
    }

    private void Player_OnTrashed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipsSO.trash, PlayerController.instance.transform.position, volume);
    }

    private void Player_OnDrop(object sender, System.EventArgs e)
    {
        PlaySound(audioClipsSO.objectDrop, PlayerController.instance.transform.position, volume);
    }

    private void Player_OnPickup(object sender, System.EventArgs e)
    {
        PlaySound(audioClipsSO.objectDrop, PlayerController.instance.transform.position, volume);
    }

    private void DeliveryManager_OnDeliverySuccess(object sender, System.EventArgs e)
    {    
        PlaySound(audioClipsSO.deliverySuccess, DeliveryManager.instance.transform.position, volume);
    }
    private void DeliveryManager_OnDeliveryFailure(object sender, System.EventArgs e)
    {
        PlaySound(audioClipsSO.deliveryFail, DeliveryManager.instance.transform.position, volume);
    }


    public void PlaySound(AudioClip[] audioClip, Vector3 position, float volume)
    {
        PlaySound(audioClip[UnityEngine.Random.Range(0,audioClip.Length - 1)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume)
    {
        AudioSource.PlayClipAtPoint(audioClip, position,volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    private AudioSource audioSource;
    private ClearCounter clearCounter;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        clearCounter = GetComponent<ClearCounter>();

    }

    private void Start()
    {
        clearCounter.OnStateChange += ClearCounter_OnStateChange;
    }

    private void ClearCounter_OnStateChange(object sender, System.EventArgs e)
    {
        if(clearCounter.isCooking == true)
        {
            audioSource.volume = SoundManager.instance.GetVolume();
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

}

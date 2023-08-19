using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;

    private AudioSource musicSource;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        
        musicSlider.onValueChanged.AddListener((v) =>
        {
            musicSource.volume = musicSlider.value;
        });
    }

}

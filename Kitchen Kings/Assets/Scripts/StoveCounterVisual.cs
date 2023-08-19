using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private GameObject stoveGlow;

    public void Visuals_ON()
    {
        particleEffect.SetActive(true);
        stoveGlow.SetActive(true);
    }

    public void Visuals_OFF()
    {
        particleEffect.SetActive(false);
        stoveGlow.SetActive(false);
    }
}

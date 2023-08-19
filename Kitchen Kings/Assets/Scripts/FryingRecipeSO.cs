using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO friedOutput;
    public KitchenObjectSO burnedOutput;

    public float fryTime;
    public float burnTime;
}

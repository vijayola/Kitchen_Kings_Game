using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;

    public KitchenObjectSO KitchenObjectSO { get { return kitchenObjectSO;} }
}

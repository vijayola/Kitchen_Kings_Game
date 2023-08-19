using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] private Plate plate;

    [Serializable]
    public struct PlateVisual
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private List<PlateVisual> plateVisuals_List;

    private void Start()
    {
        plate.On_KitchenObj_AddedtoPlate += Plate_On_KitchenObj_AddedtoPlate;
    }

    private void Plate_On_KitchenObj_AddedtoPlate(object sender, Plate.On_KitchenObj_AddedtoPlate_EventArgs e)
    {
        int n = plateVisuals_List.Count; 

        for(int i=0; i < n; i++)
        {
            if(e.kitchenObjectSO == plateVisuals_List[i].kitchenObjectSO)
            {
                plateVisuals_List[i].gameObject.SetActive(true);
            }
        }
    }
}

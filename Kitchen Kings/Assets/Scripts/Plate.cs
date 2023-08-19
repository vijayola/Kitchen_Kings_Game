using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{

    public event EventHandler<On_KitchenObj_AddedtoPlate_EventArgs> On_KitchenObj_AddedtoPlate;   // use event to show a visual on 'Plate Complete Visual' and passs the KO_SO to find which KO is added
    public class On_KitchenObj_AddedtoPlate_EventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> valid_KO_list; // the KOs that can be kept on the plate

    private List<KitchenObjectSO> KO_onPlate_list;   // KOs currently kept on plate

    private void Awake()
    {
        //valid_KO_list = new List<KitchenObjectSO>();  // not required in public or seralised field *imp

        KO_onPlate_list = new List<KitchenObjectSO>();
    }

    public List<KitchenObjectSO> Valid_KO_list_Fun()
    {
        return valid_KO_list;
    }

    public List<KitchenObjectSO> Get_KO_onPlate_list_Fun()
    {
        return KO_onPlate_list;
    }

    public void Add_KO_onPlate_Fun(KitchenObjectSO KO_SO)
    {
        On_KitchenObj_AddedtoPlate?.Invoke(this, new On_KitchenObj_AddedtoPlate_EventArgs
        {
            kitchenObjectSO = KO_SO
        }) ;

        KO_onPlate_list.Add(KO_SO);
    }
}

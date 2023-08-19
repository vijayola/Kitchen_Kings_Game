using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private Plate plate;

    [SerializeField] private GameObject iconTemplate;


    private void Start()
    {
        plate.On_KitchenObj_AddedtoPlate += Plate_On_KitchenObj_AddedtoPlate;
    }

    private void Plate_On_KitchenObj_AddedtoPlate(object sender, Plate.On_KitchenObj_AddedtoPlate_EventArgs e)
    {
        GameObject icon_Template = Instantiate(iconTemplate, transform);
        icon_Template.SetActive(true);

        icon_Template.transform.Find("Icon").GetComponent<Image>().sprite = e.kitchenObjectSO.sprite;

    }

}

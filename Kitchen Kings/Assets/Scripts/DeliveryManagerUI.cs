using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject recipeTemplate;

    [SerializeField] private GameObject iconsUI;
    [SerializeField] private GameObject iconTemplate;

    [SerializeField] private DeliveryManager deliveryManager;

    [SerializeField] private RecipeNameTextUI recipeNameTextUI;


    private void Start()
    {
        deliveryManager.OnRecipeAdded_to_WaitingList += DeliveryManager_OnRecipeAdded_to_WaitingList;

        deliveryManager.OnDeliveryCompleted += DeliveryManager_OnDeliveryCompleted;
    }

    private void DeliveryManager_OnDeliveryCompleted(object sender, System.EventArgs e)
    {
        UpdateUI_Canvas();
    }

    private void DeliveryManager_OnRecipeAdded_to_WaitingList(object sender, System.EventArgs e)
    {
        UpdateUI_Canvas();
    }

    // ** Better method of doing it is shown in video 
    private void UpdateUI_Canvas()
    {
        foreach(Transform t in container.transform)
        {
            if(t == recipeTemplate.transform)
            {
                continue;
            }
            Destroy(t.gameObject);
        }

        /*foreach (Transform t in iconsUI.transform)
        {
            if (t == iconTemplate.transform)
            {
                continue;
            }
            Destroy(t.gameObject);
        }*/

        int n = deliveryManager.Get_Waiting_RecipeList_Fun().Count;

        for(int i = 0; i < n; i++)
        {
            GameObject recipe_Template = Instantiate(recipeTemplate, container.transform);   // spawn recipe template

            recipe_Template.SetActive(true);

            //recipe_Template.transform.Find("RecipeNameText").GetComponent<TMP_InputField>().text = deliveryManager.Get_Waiting_RecipeList_Fun()[i].Name;  // set the name on UI

            recipe_Template.transform.Find("RecipeNameText").GetComponent<RecipeNameTextUI>().Set_recipeNameText_Fun(deliveryManager.Get_Waiting_RecipeList_Fun()[i].Name);

            Debug.Log("recipe added = " + deliveryManager.Get_Waiting_RecipeList_Fun()[i].Name);

            List<KitchenObjectSO> kitchenObjectSOs = deliveryManager.Get_Waiting_RecipeList_Fun()[i].kitchenObjectSOs;  // for spawning icons

            for (int j = 0; j < kitchenObjectSOs.Count; j++)
            {
                GameObject icon_Template = Instantiate(iconTemplate , recipe_Template.transform.Find("IconsUI").transform);
                icon_Template.SetActive(true);

                icon_Template.transform.Find("Icon").GetComponent<Image>().sprite = kitchenObjectSOs[j].sprite;
                Debug.Log(kitchenObjectSOs[j].objectName);

            }

        }


    }

}

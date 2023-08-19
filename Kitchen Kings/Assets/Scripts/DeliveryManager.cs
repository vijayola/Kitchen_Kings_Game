using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager instance;


    public event EventHandler OnRecipeAdded_to_WaitingList;
    public event EventHandler OnDeliveryCompleted;


    public event EventHandler OnDeliverySuccess;
    public event EventHandler OnDeliveryFailure;

    [Serializable]
    public struct Recipe
    {
        public string Name;
        public List<KitchenObjectSO> kitchenObjectSOs;
    }

    [SerializeField] private List<Recipe> recipeList;

    private float timer = 0;
    private float timerMax = 4f;

    private float recipes = 0;
    private float recipesMax = 4f;

    private List<Recipe> waitingRecipeList;


    private int recipesDelivered = 0;

    private void Awake()
    {
        instance = this;

        waitingRecipeList = new List<Recipe>();
    }
    private void Update()
    {
        if(recipes < recipesMax)
        {
            timer += Time.deltaTime;
            if(timer > timerMax)
            {
             
                Recipe recipe = recipeList[UnityEngine.Random.Range(0, recipeList.Count)];

                Debug.Log(recipe.Name);

                waitingRecipeList.Add(recipe);

                OnRecipeAdded_to_WaitingList?.Invoke(this, EventArgs.Empty);     // update the UI

                timer = 0;
                recipes++;

            }
        }
    }

    public void DeliverRecipe(List<KitchenObjectSO> kitchenObjectSOs)
    {
        for(int i = 0; i < waitingRecipeList.Count; i++)
        {
            List<KitchenObjectSO> recipe = waitingRecipeList[i].kitchenObjectSOs;

            for(int j = 0; j < recipe.Count; j++)
            {
                KitchenObjectSO kitchenObjectSO = recipe[j];

                bool ingredientFound = false;

                if(recipe.Count != kitchenObjectSOs.Count)    // if the size of 2 lists is not equal then do not proceed
                {
                    ingredientFound = false;
                    break;
                }

                for(int k = 0; k < kitchenObjectSOs.Count; k++)
                {
                    if (recipe[j] == kitchenObjectSOs[k])
                    {
                        ingredientFound = true;
                        break;
                    }
                }

                if(ingredientFound == false)
                {
                    break;
                }else if(j == recipe.Count - 1)
                {
                    Debug.Log(waitingRecipeList[i].Name + "Delivered !!");

                    recipesDelivered++;

                    recipes--;             // now we can add more recipes

                    waitingRecipeList.RemoveAt(i);   // since it is delivered successfully

                    OnDeliveryCompleted?.Invoke(this, EventArgs.Empty);
                    OnDeliverySuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }

        }
        OnDeliveryFailure?.Invoke(this, EventArgs.Empty);   

        Debug.Log("Wrong Delivery !!");
    }

    public List<Recipe> Get_Waiting_RecipeList_Fun()
    {
        return waitingRecipeList;
    }

    public int RecipesDelivered()
    {
        return recipesDelivered;
    }
}

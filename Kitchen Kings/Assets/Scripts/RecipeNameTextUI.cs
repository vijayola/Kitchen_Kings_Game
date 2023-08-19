using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeNameTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;

    public string Get_recipeNameText_Fun()
    {
        return recipeNameText.text;
    }

    public void Set_recipeNameText_Fun(string s)
    {
        recipeNameText.text = s;
    }
}

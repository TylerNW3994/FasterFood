using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingRecipe", menuName = "ScriptableObjects/CookingRecipeSO", order = 1)]
public class CookingRecipeSO : ScriptableObject {
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float cookingTime;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FullRecipe", menuName = "ScriptableObjects/FullRecipeSO", order = 1)]
public class FullRecipeSO : ScriptableObject {
    public List<KitchenObjectSO> input;
    public bool isServable;
    public int price;
}

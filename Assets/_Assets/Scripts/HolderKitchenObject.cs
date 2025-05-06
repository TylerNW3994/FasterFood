using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderKitchenObject : KitchenObject {
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<FullRecipeSO> allowedRecipes;
    private List<FullRecipeSO> possibleRecipes;
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake() {
        kitchenObjectSOList = new();
        possibleRecipes = allowedRecipes;
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {
        // Avoid Duplication
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            return false;
        }

        List<FullRecipeSO> newPossibleRecipes = new();
        foreach (FullRecipeSO recipeSO in possibleRecipes) {
            if (recipeSO.input.Contains(kitchenObjectSO)) {
                newPossibleRecipes.Add(recipeSO);
            }
        }

        if (newPossibleRecipes.Count == 0) {
            // Invalid ingredient insertion
            return false;
        }

        possibleRecipes = newPossibleRecipes;

        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
            kitchenObjectSO = kitchenObjectSO
        });
        return true;
    }
}

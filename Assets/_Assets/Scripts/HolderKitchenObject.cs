using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderKitchenObject : KitchenObject {
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake() {
        kitchenObjectSOList = new();
    }

    public void AddIngredient(KitchenObjectSO kitchenObjectSO) {
        kitchenObjectSOList.Add(kitchenObjectSO);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Pickup(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
            } else {
                if (player.GetKitchenObject().TryGetHolder(out HolderKitchenObject holderKitchenObject)) {
                    holderKitchenObject.AddIngredient(GetKitchenObject().GetKitchenObjectSO());
                    GetKitchenObject().DestroySelf();
                }
            }
        }
    }
}

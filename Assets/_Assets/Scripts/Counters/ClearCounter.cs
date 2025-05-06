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
                    holderKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO());
                    GetKitchenObject().DestroySelf();
                } else {
                    if (GetKitchenObject().TryGetHolder(out holderKitchenObject)) {
                        if (holderKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }
}

using UnityEngine;

public class CuttingCounter : BaseCounter {
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private float cuttingTimer;

    public override void Pickup(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                cuttingTimer = 0f;
                player.GetKitchenObject().SetKitchenObjectParent(this);

                if (HasRecipe(GetKitchenObject().GetKitchenObjectSO()) == false) {
                    return;
                }

                InvokeOnProgressChanged(new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = cuttingTimer / GetRecipe(GetKitchenObject().GetKitchenObjectSO()).cuttingTime
                });
            }
        } else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
            } else {
                if (player.GetKitchenObject().TryGetHolder(out HolderKitchenObject holderKitchenObject)) {
                    if (holderKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
    }

    public override void Interact(Player player) {
        if (HasKitchenObject()) {
            if (!player.HasKitchenObject()) {
                KitchenObjectSO output = GetRecipeOutput(GetKitchenObject().GetKitchenObjectSO());
                if (output == null) return;

                CuttingRecipeSO cuttingRecipeSO = GetRecipe(GetKitchenObject().GetKitchenObjectSO());
                
                cuttingTimer++;
                InvokeOnCut();
                InvokeOnProgressChanged(new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = cuttingTimer / GetRecipe(GetKitchenObject().GetKitchenObjectSO()).cuttingTime
                });
                if (cuttingTimer >= cuttingRecipeSO.cuttingTime) {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(output, this);
                }
            }
        }
    }

    private bool HasRecipe(KitchenObjectSO input) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == input) {
                return true;
            }
        }
        return false;
    }

    private KitchenObjectSO GetRecipeOutput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetRecipe(inputKitchenObjectSO);
        if (cuttingRecipeSO == null) return null;
        return cuttingRecipeSO.output;
    }

    private CuttingRecipeSO GetRecipe(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}

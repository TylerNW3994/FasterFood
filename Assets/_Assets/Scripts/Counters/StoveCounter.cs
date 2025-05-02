using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter {
    private enum State {
        Idle,
        Cooking,
        Cooked,
        Burned
    }
    [SerializeField] private CookingRecipeSO[] cookingRecipeSOArray;

    private State state;
    private float cookingTime;
    private CookingRecipeSO cookingRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Cooking:
                    cookingTime += Time.deltaTime;
                    cookingRecipeSO = GetRecipeSO(GetKitchenObject().GetKitchenObjectSO());

                    if (cookingTime > cookingRecipeSO.cookingTime) {
                        cookingTime = 0f;
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(cookingRecipeSO.output, this);
                        state = State.Cooked;
                    }
                    break;
                case State.Cooked:
                    break;
                case State.Burned:
                    break;
            }
        }
    } 

    public override void Pickup(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                cookingTime = 0f;
                player.GetKitchenObject().SetKitchenObjectParent(this);
                state = State.Cooking;

                if (HasRecipe(GetKitchenObject().GetKitchenObjectSO()) == false) {
                    return;
                }

                InvokeOnProgressChanged(new OnProgressChangedEventArgs {
                    progressNormalized = cookingTime / GetRecipeSO(GetKitchenObject().GetKitchenObjectSO()).cookingTime
                });
            }
        } else {
            if (!player.HasKitchenObject()) {
                state = State.Idle;
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void Interact(Player player) {
        if (HasKitchenObject()) {
            if (!player.HasKitchenObject()) {
                KitchenObjectSO output = GetRecipeOutput(GetKitchenObject().GetKitchenObjectSO());
                if (output == null) return;

                cookingRecipeSO = GetRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                
                cookingTime++;
                InvokeOnCut();
                InvokeOnProgressChanged(new OnProgressChangedEventArgs {
                    progressNormalized = cookingTime / GetRecipeSO(GetKitchenObject().GetKitchenObjectSO()).cookingTime
                });
                if (cookingTime >= cookingRecipeSO.cookingTime) {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(output, this);
                }
            }
        }
    }

    private bool HasRecipe(KitchenObjectSO input) {
        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipeSOArray) {
            if (cookingRecipeSO.input == input) {
                return true;
            }
        }
        return false;
    }

    private KitchenObjectSO GetRecipeOutput(KitchenObjectSO inputKitchenObjectSO) {
        cookingRecipeSO = GetRecipeSO(inputKitchenObjectSO);
        if (cookingRecipeSO == null) return null;
        return cookingRecipeSO.output;
    }

    private CookingRecipeSO GetRecipeSO(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipeSOArray) {
            if (cookingRecipeSO.input == inputKitchenObjectSO) {
                return cookingRecipeSO;
            }
        }
        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter {
    public event EventHandler<OnCookingStateChangedEventArgs> CookingStateChanged;

    public class OnCookingStateChangedEventArgs : EventArgs {
        public State state;
    }

    public enum State {
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

                    if (cookingTime > cookingRecipeSO.cookingTime) {
                        state = State.Cooked;
                    }
                    break;
                case State.Cooked:
                    cookingTime = 0f;
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(cookingRecipeSO.output, this);
                    cookingRecipeSO = GetCookingRecipeSO(cookingRecipeSO.output);

                    state = cookingRecipeSO ? State.Cooking : State.Burned;

                    CookingStateChanged?.Invoke(this, new OnCookingStateChangedEventArgs {
                        state = state
                    });
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
                CookingStateChanged?.Invoke(this, new OnCookingStateChangedEventArgs {
                    state = state
                });

                if (HasRecipe(GetKitchenObject().GetKitchenObjectSO()) == false) {
                    return;
                }
                
                cookingRecipeSO = GetCookingRecipeSO(GetKitchenObject().GetKitchenObjectSO());

                InvokeOnProgressChanged(new OnProgressChangedEventArgs {
                    progressNormalized = cookingTime / cookingRecipeSO.cookingTime
                });
            }
        } else {
            if (!player.HasKitchenObject()) {
                state = State.Idle;
                CookingStateChanged?.Invoke(this, new OnCookingStateChangedEventArgs {
                    state = state
                });
                GetKitchenObject().SetKitchenObjectParent(player);
                cookingRecipeSO = null;
            }
        }
    }

    public override void Interact(Player player) {
        if (HasKitchenObject()) {
            if (!player.HasKitchenObject()) {
                KitchenObjectSO output = GetRecipeOutput(GetKitchenObject().GetKitchenObjectSO());
                if (output == null) return;

                cookingRecipeSO = GetCookingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                
                cookingTime++;
                InvokeOnCut();
                InvokeOnProgressChanged(new OnProgressChangedEventArgs {
                    progressNormalized = cookingTime / GetCookingRecipeSO(GetKitchenObject().GetKitchenObjectSO()).cookingTime
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
        cookingRecipeSO = GetCookingRecipeSO(inputKitchenObjectSO);
        if (cookingRecipeSO == null) return null;
        return cookingRecipeSO.output;
    }

    private CookingRecipeSO GetCookingRecipeSO(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipeSOArray) {
            if (cookingRecipeSO.input == inputKitchenObjectSO) {
                return cookingRecipeSO;
            }
        }
        return null;
    }
}

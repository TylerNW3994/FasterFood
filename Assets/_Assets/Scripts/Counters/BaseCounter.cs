using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;

    protected void InvokeOnProgressChanged(IHasProgress.OnProgressChangedEventArgs args) {
        OnProgressChanged?.Invoke(this, args);
    }
    protected void InvokeOnCut() {
        OnCut?.Invoke(this, EventArgs.Empty);
    }

    [SerializeField] private Transform kitchenObjectHoldPoint;

    private KitchenObject kitchenObject;
    public virtual void Interact(Player player) {}
    public virtual void Pickup(Player player) {}

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}

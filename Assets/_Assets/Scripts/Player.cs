using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {
    private KitchenObject kitchenObject;

    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking = false;
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("More than one Player instance found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnPickupAction += GameInput_OnPickupAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (selectedCounter != null) { 
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnPickupAction(object sender, System.EventArgs e) {
        if (selectedCounter != null) { 
            selectedCounter.Pickup(this);
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() { 
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 movementDirection = new Vector3(inputVector.x, 0, inputVector.y);
        float interactionDistance = 2f;

        if (movementDirection != Vector3.zero) {
            lastInteractDirection = movementDirection;
        }

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactionDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 movementDirection = new Vector3(inputVector.x, 0, inputVector.y);

        float playerSize = 0.7f, 
            playerHeight = 2f, 
            moveDistance = moveSpeed * Time.deltaTime;
        Vector3 capsuleSize = transform.position + Vector3.up * playerHeight;

        bool canMove = !Physics.CapsuleCast(transform.position, capsuleSize, playerSize, movementDirection, moveDistance);

        if (!canMove) {
            Vector3 moveX = new Vector3(movementDirection.x, 0, 0).normalized;
            canMove = moveX != Vector3.zero && !Physics.CapsuleCast(transform.position, capsuleSize, playerSize, moveX, moveDistance);
            if (canMove) {
                movementDirection = moveX;
            } else {
                Vector3 moveZ = new Vector3(0, 0, movementDirection.z).normalized;
                canMove = moveX != Vector3.zero && !Physics.CapsuleCast(transform.position, capsuleSize, playerSize, moveZ, moveDistance);
                if (canMove) {
                    movementDirection = moveZ;
                }
            }   
        }

        if (canMove) {
            transform.position += movementDirection * moveDistance;
        }

        isWalking = movementDirection != Vector3.zero;
        
        transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter baseCounter) {
        selectedCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

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

    public void Interact(Player player) {
        // Implement interaction logic here
    }
}

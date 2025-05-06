using System;
using UnityEngine;

public class PlatesCounter : BaseCounter {
    public event EventHandler OnHolderSpawned;
    public event EventHandler OnHolderRemoved;
    [SerializeField] private KitchenObjectSO holderKitchenObject;
    private float spawnTimer, spawnTimerMax = 4f;
    private int holdersSpawned, holdersSpawnedMax = 4;

    private void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTimerMax) {
            spawnTimer = 0f;

            if (holdersSpawned < holdersSpawnedMax) {
                holdersSpawned++;

                OnHolderSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Pickup(Player player) {
        if (!player.HasKitchenObject()) {
            if (holdersSpawned > 0) {
                holdersSpawned--;
                KitchenObject.SpawnKitchenObject(holderKitchenObject, player);
                OnHolderRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

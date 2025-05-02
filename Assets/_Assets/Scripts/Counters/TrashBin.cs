using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashBin : BaseCounter {
    public override void Pickup(Player player) {
        player.GetKitchenObject()?.DestroySelf();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKitchenObject", menuName = "ScriptableObjects/KitchenObject")]
public class KitchenObjectSO : ScriptableObject {
    public Transform prefab;
    public Sprite iconSprite;
    public string objectName;
}

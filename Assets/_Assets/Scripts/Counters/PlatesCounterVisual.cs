using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {
    [SerializeField] private PlatesCounter holderCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform visualPrefab;

    private List<GameObject> visualGameObjectList;

    private void Awake() {
        visualGameObjectList = new();
    }

    private void Start() {
        holderCounter.OnHolderSpawned += PlatesCounter_OnHolderSpawned;
        holderCounter.OnHolderRemoved += PlatesCounter_OnHolderRemoved;
    }

    private void PlatesCounter_OnHolderSpawned(object sender, EventArgs e) {
        Transform visualTransform = Instantiate(visualPrefab, counterTopPoint);

        float holderYOffest = .1f;
        visualTransform.localPosition = new Vector3(0, holderYOffest * visualGameObjectList.Count, 0);

        visualGameObjectList.Add(visualTransform.gameObject);
    }

    private void PlatesCounter_OnHolderRemoved(object sender, EventArgs e) {
        GameObject holderGameObject = visualGameObjectList[visualGameObjectList.Count - 1];
        visualGameObjectList.Remove(holderGameObject);
        Destroy(holderGameObject);
    }
}

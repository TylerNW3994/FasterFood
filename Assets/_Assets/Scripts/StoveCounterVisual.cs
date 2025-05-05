using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGO;
    [SerializeField] private GameObject particlesGO;

    private void Start() {
        stoveCounter.CookingStateChanged += StoveCounter_CookingStateChanged;
    }

    private void StoveCounter_CookingStateChanged(object sender, StoveCounter.OnCookingStateChangedEventArgs e) {
        bool showVisual = e.state == StoveCounter.State.Cooking;
        stoveOnGO.SetActive(showVisual);
        particlesGO.SetActive(showVisual);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private BaseCounter counter;
    [SerializeField] private Image barImage;

    private void Start() {
        counter.OnProgressChanged += BaseCounter_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }
    
    private void BaseCounter_OnProgressChanged(object sender, BaseCounter.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }   
}

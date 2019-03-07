using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstBar : MonoBehaviour {
    public int maxValue;
    public int value;

    private Text conditionText;
    private Slider slider;

    private ThirstController thirstController;

    private void Awake() {
        conditionText = GetComponentInChildren<Text>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        thirstController = ThirstController.instance;

        value = (int) thirstController.GetThirst;
    }

    private void Update() {
        conditionText.text = value + "/" + maxValue;
        slider.value = (float)value / maxValue;
    }
}

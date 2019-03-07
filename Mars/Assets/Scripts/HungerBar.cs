using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour {
    public int maxValue;
    public int value;

    private Text conditionText;
    private Slider slider;

    private HungerController hungerController;

    private void Awake() {
        conditionText = GetComponentInChildren<Text>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        hungerController = HungerController.instance;

        maxValue = hungerController.maxHunger;
        value = (int) hungerController.hunger;
    }

    private void Update() {
        value = (int) hungerController.hunger;
        conditionText.text = value + "/" + maxValue;
        slider.value = (float)value / maxValue;
    }
}

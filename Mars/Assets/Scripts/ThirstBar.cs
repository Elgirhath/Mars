﻿using System.Collections;
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

        maxValue = thirstController.maxThirst;
        value = (int) thirstController.thirst;
    }

    private void Update() {
        value = (int) thirstController.thirst;
        conditionText.text = value + "/" + maxValue;
        slider.value = (float)value / maxValue;
    }
}
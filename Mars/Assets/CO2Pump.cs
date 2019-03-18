using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO2Pump : MonoBehaviour {
    public float kgPerSec;
    public float activateThreshold;

    private CapsuleAirController airController;
    // Start is called before the first frame update
    void Start() {
        airController = GetComponentInParent<CapsuleAirController>();
    }

    // Update is called once per frame
    void Update() {
        if (Mathf.Abs(airController.co2Mass - airController.co2TargetMass) > activateThreshold) {
            float maxAmount = airController.co2TargetMass - airController.co2Mass;
            float amount = Mathf.Sign(maxAmount) * kgPerSec;
            airController.co2Mass += Mathf.Clamp(amount * Time.deltaTime, -Mathf.Abs(maxAmount), Mathf.Abs(maxAmount));
        }
    }
}
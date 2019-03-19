using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO2Pump : MonoBehaviour {
    public float kgPerSec;
    public float activateThreshold;

    public Gas co2;

    private CapsuleAirController airController;
    // Start is called before the first frame update
    void Start() {
        airController = GetComponentInParent<CapsuleAirController>();
    }

    // Update is called once per frame
    void Update() {
        if (Mathf.Abs(co2.GetMass(airController.air) - co2.GetMass(airController.targetAir)) > activateThreshold) {
            float maxAmount = co2.GetMass(airController.targetAir) - co2.GetMass(airController.air);
            float amount = Mathf.Sign(maxAmount) * kgPerSec * Time.deltaTime;
            float amountClamped = Mathf.Clamp(amount, -Mathf.Abs(maxAmount), Mathf.Abs(maxAmount));
            float currentMass = co2.GetMass(airController.air);
            float newMass = currentMass + amountClamped;
            co2.SetMass(airController.air, newMass);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2Pump : MonoBehaviour
{
    public float kgPerSec;
    public float activateThreshold;

    public Gas o2;
    private GasTank tank;

    private CapsuleAirController airController;
    // Start is called before the first frame update
    void Start() {
        airController = GetComponentInParent<CapsuleAirController>();
        tank = GetComponentInChildren<GasTank>();
    }

    // Update is called once per frame
    void Update() {
        float currentPartialPressure = o2.GetPartialPressure(airController.air);
        float targetPartialPressure = o2.GetPartialPressure(airController.targetAir);

        if (Mathf.Abs(targetPartialPressure - currentPartialPressure) > activateThreshold) {
            float amount = Mathf.Sign(targetPartialPressure - currentPartialPressure) * kgPerSec * Time.deltaTime;
//            float amountClamped = Mathf.Clamp(amount, -Mathf.Abs(maxAmount), Mathf.Abs(maxAmount));
            amount = amount > tank.gasWeight ? tank.gasWeight : amount;
            float currentMass = o2.GetMass(airController.air);
            float newMass = currentMass + amount;
            o2.SetMass(airController.air, newMass);
            tank.gasWeight -= amount;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO2Pump : MonoBehaviour {
    public float kgPerSec;
    public float activateThreshold;

    public Gas co2;

    private CapsuleAirController airController;
    private PowerSocket powerSocket;
    
    void Start() {
        airController = GetComponentInParent<CapsuleAirController>();
        powerSocket = GetComponent<PowerSocket>();
    }

    // Update is called once per frame
    void Update() {
        if (!powerSocket.IsPowered())
            return;
        
        float currentMass = co2.GetMass(airController.air);
        float targetMass = co2.GetMass(airController.targetAir);
        
        if (Mathf.Abs(currentMass - targetMass) < activateThreshold)
            return;
        
        float maxAmount = targetMass - currentMass;
        float amount = Mathf.Sign(maxAmount) * kgPerSec * Time.deltaTime;
        float amountClamped = Mathf.Clamp(amount, -Mathf.Abs(maxAmount), Mathf.Abs(maxAmount));
        float newMass = currentMass + amountClamped;
        co2.SetMass(airController.air, newMass);
    }
}
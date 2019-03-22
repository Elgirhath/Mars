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
    private PowerSocket powerSocket;
    
    void Start() {
        airController = GetComponentInParent<CapsuleAirController>();
        tank = GetComponentInChildren<GasTank>();
        powerSocket = GetComponent<PowerSocket>();
    }

    // Update is called once per frame
    void Update() {
        if (!powerSocket.IsPowered())
            return;
        
        float currentPartialPressure = o2.GetPartialPressure(airController.air);
        float targetPartialPressure = o2.GetPartialPressure(airController.targetAir);

        if (!(Mathf.Abs(targetPartialPressure - currentPartialPressure) > activateThreshold))
            return;
        
        float amount = Mathf.Sign(targetPartialPressure - currentPartialPressure) * kgPerSec * Time.deltaTime;
        amount = amount > tank.gasWeight ? tank.gasWeight : amount;
        float currentMass = o2.GetMass(airController.air);
        float newMass = currentMass + amount;
        o2.SetMass(airController.air, newMass);
        tank.gasWeight -= amount;
    }
}

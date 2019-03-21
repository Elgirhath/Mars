using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSocket : MonoBehaviour {
    private PowerSupply powerSupply;
    public float powerConsumption;

    public float powerReceived {
        get => GetPower();
    }
    
    public float GetPower() {
        return powerSupply.GetPower(this);
    }

    private void Start() {
        powerSupply = transform.parent.GetComponentInChildren<PowerSupply>();
        powerSupply.receivers.Add(this);
    }
}

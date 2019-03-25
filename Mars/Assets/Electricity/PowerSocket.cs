using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSocket : MonoBehaviour {
    private PowerSupply powerSupply;
    public float powerConsumption;
    public uint priority;

    public bool isPowered {
        get => powerSupply.IsPowered(this);
    }

    private void Start() {
        powerSupply = transform.parent.GetComponentInChildren<PowerSupply>();
        powerSupply.AddReceiver(this);
    }
}

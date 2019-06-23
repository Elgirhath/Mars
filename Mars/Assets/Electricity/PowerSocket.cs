using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSocket : MonoBehaviour {
    private PowerSupply powerSupply;
    public float powerConsumption;
    public uint priority;

    public bool isPowered {
        get => powerSupply?.IsPowered(this) ?? false;
    }

    private void Start() {
        powerSupply = PowerSupply.instance;
        powerSupply.AddReceiver(this);
    }

    private void OnDrawGizmos() {
        Color color = isPowered ? Color.green : Color.red;
        color.a = 0.6f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position + Vector3.up * 2f, 0.4f);
    }
}

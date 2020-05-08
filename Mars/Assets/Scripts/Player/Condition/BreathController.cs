using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreathController : MonoBehaviour {
    public int maxOxygen;

    public int initOxygen;

    [Tooltip("Points per second")] public float dropSpeed;

    private float _oxygen;
    public float oxygen {
        get => _oxygen;
        set {
            _oxygen = value;
            oxygenBar.ChangeOxygenBar((int) oxygen);
        }

    }

    private Air air;

    public bool insideCapsule { get; set; }

    public static BreathController instance;
    
    private OxygenBar oxygenBar;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start() {
        air = null;
        _oxygen = initOxygen;
        oxygenBar = OxygenBar.instance;
        insideCapsule = false;

        oxygenBar.maxValue = maxOxygen;
        oxygenBar.value = (int) oxygen;
    }

    private void Update()
    {
        air = GetAirZone()?.air;

        if (air != null)
            return;
        
        if (_oxygen <= 0.0f) {
            Debug.Log("You are dead!");
        }
        else {
            oxygen -= dropSpeed * Time.deltaTime;
        }
    }

    private AirZone GetAirZone()
    {
        return Zone.GetZonesInPoint<AirZone>(transform.position).FirstOrDefault();
    }
}

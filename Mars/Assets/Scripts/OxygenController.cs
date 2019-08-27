using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenController : MonoBehaviour {
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

    [NonSerialized]
    public Air air;

    public bool insideCapsule { get; set; }

    public static OxygenController instance;
    
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

    private void Update() {
        if (air != null)
            return;
        
        if (_oxygen <= 0.0f) {
            Debug.Log("You are dead!");
        }
        else {
            oxygen -= dropSpeed * Time.deltaTime;
        }
    }
}

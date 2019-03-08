using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerController : MonoBehaviour {
    public int maxHunger;
    public int initHunger;

    private float _hunger;
    public float hunger {
        get => _hunger;
        set => _hunger = value;
    }
	
    [Tooltip("Points per second")]
    public float dropSpeed;

    public static HungerController instance;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start() {
        _hunger = initHunger;
    }

    private void Update() {
        _hunger -= dropSpeed * Time.deltaTime;

        if (_hunger <= 0.0f) {
            Debug.Log("U ded");
        }
    }
}

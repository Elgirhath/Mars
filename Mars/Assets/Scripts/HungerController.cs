using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerController : MonoBehaviour {
    public int maxHunger;
    public int initHunger;
    public float starvationDamage;
    public float recoverySpeed;
    public float recoveryThreshold;

    private float _hunger;
    public float hunger {
        get => _hunger;
        set => _hunger = value;
    }
	
    [Tooltip("Points per second")]
    public float dropSpeed;

    public static HungerController instance;

    private HealthController healthController;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start() {
        _hunger = initHunger;
        healthController = HealthController.instance;
    }

    private void Update() {
        if (_hunger <= 0.0f) {
            Debug.Log("You are dying out of hunger!");
            healthController.ChangeHealth(-starvationDamage  * Time.deltaTime);
        }
        else
        {
            _hunger -= dropSpeed * Time.deltaTime;
            
            if (healthController.health < healthController.maxHealth && _hunger >= recoveryThreshold * maxHunger)
            {
                Debug.Log("You are well fed, so you are slowly regaining your health.");
                healthController.ChangeHealth(recoverySpeed  * Time.deltaTime);
            }
        }
    }
}

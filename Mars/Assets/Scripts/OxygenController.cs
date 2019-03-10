using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenController : MonoBehaviour {
    public int maxOxygen;
    public int initOxygen;
    public int chokingDamage;
    [Tooltip("Points per second")]
    public float dropSpeed;
    
    private float _oxygen;
    public float oxygen {
        get => _oxygen;
        set => _oxygen = value;
    }

    private bool _insideCapsule;

    public bool insideCapsule
    {
        get => _insideCapsule;
        set => _insideCapsule = value;
    }

    public static OxygenController instance;
    
    private HealthController healthController;
    private OxygenBar oxygenBar;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start() {
        _oxygen = initOxygen;
        healthController = HealthController.instance;
        oxygenBar = OxygenBar.instance;
        insideCapsule = false;

        oxygenBar.maxValue = maxOxygen;
        oxygenBar.value = (int)oxygen;
    }

    private void Update()
    {
        if (!insideCapsule)
        {
            _oxygen -= dropSpeed * Time.deltaTime;
            if (_oxygen <= 0.0f) {
                Debug.Log("You are choking to death!");
                healthController.ChangeHealth(-chokingDamage);
            }
            oxygenBar.ChangeOxygenBar((int) oxygen);
        }
    }
}

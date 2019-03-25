using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heater : MonoBehaviour {
    public float temperature;
    public float area;
    
    private CapsuleAirController airController;
    private PowerSocket powerSocket;

    private bool active {
        get {
            bool isPowered = powerSocket.isPowered;
            bool isTriggered = airController.air.temperature < airController.targetAir.temperature;
            return isPowered && isTriggered;
        }
    }

    private float emmisivity = 0.5f;
    private float boltzmanConstant = 5.670367e-8f;
    private float convectiveCoefficient = 20f;
    
    void Start() {
        airController = GetComponentInParent<CapsuleAirController>();
        powerSocket = GetComponent<PowerSocket>();
    }

    // Update is called once per frame
    void Update() {
        if (!powerSocket.isPowered)
            return;

        float currentTemp = airController.air.temperature;
        float targetTemp = airController.targetAir.temperature;
        
        if (currentTemp >= targetTemp)
            return;
        
        float diff = Mathf.Pow(temperature, 4) - Mathf.Pow(currentTemp, 4);
        float radiativeHeat = area * boltzmanConstant * emmisivity * diff;
        float convectiveHeat = convectiveCoefficient * area * (temperature - currentTemp);

        float dTdt = (radiativeHeat + convectiveHeat) / (airController.air.GetMass() * airController.air.heatCapacity);
        airController.air.temperature += dTdt * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heater : MonoBehaviour {
    public float temperature;
    public float area;
    
    private CapsuleAirController airController;

    private float emmisivity = 0.5f;
    private float boltzmanConstant = 5.670367e-8f;
    private float convectiveCoefficient = 20f;
    
    void Start() {
        airController = GetComponentInParent<CapsuleAirController>();
    }

    // Update is called once per frame
    void Update() {
        if (airController.temperature < airController.temperatureTarget) {
            float diff = Mathf.Pow(temperature, 4) - Mathf.Pow(airController.temperature, 4);
            float radiativeHeat = area * boltzmanConstant * emmisivity * diff;
            float convectiveHeat = convectiveCoefficient * area * (temperature - airController.temperature);

            float dTdt = (radiativeHeat + convectiveHeat) / (airController.GetMass() * airController.heatCapacity);
            airController.temperature = airController.temperature + dTdt * Time.deltaTime;
        }
    }
}

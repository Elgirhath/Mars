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
        if (airController.air.temperature < airController.targetAir.temperature) {
            float airTemp = airController.air.temperature;
            float diff = Mathf.Pow(temperature, 4) - Mathf.Pow(airTemp, 4);
            float radiativeHeat = area * boltzmanConstant * emmisivity * diff;
            float convectiveHeat = convectiveCoefficient * area * (temperature - airTemp);

            float dTdt = (radiativeHeat + convectiveHeat) / (airController.air.GetMass() * airController.air.heatCapacity);
            airController.air.temperature += dTdt * Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPump : MonoBehaviour
{
    public float kgPerSec;
    public float activateThreshold;

    public Gas n2;
    private GasTank tank;

    private CapsuleAirController airController;
    private PowerSocket powerSocket;
    
    void Start() {
        airController = GetComponentInParent<CapsuleAirController>();
        tank = GetComponentInChildren<GasTank>();
        powerSocket = GetComponent<PowerSocket>();
    }

    // Update is called once per frame
    void Update() {
        if (!powerSocket.IsPowered())
            return;
        
        Air currentAir = airController.air;
        Air targetAir = airController.targetAir;
        
        if (!(Mathf.Abs(targetAir.pressure - currentAir.pressure) > activateThreshold))
            return;
        
        float amount = Mathf.Sign(targetAir.pressure - currentAir.pressure) * kgPerSec * Time.deltaTime;
        amount = amount > tank.gasWeight ? tank.gasWeight : amount;
        float currentMass = n2.GetMass(airController.air);
        float newMass = currentMass + amount;
        n2.SetMass(airController.air, newMass);
        tank.gasWeight -= amount;
    }
}

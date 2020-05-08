using System.Collections;
using UnityEngine;

public class O2Pump : Pump
{
    public float kgPerSec;
    public float activateThreshold;

    public Gas o2;
    private GasTank tank;

    private CapsuleAirController airController;

    protected override void OnPumpStart()
    {
        airController = GetComponentInParent<CapsuleAirController>();
        tank = GetComponentInChildren<GasTank>();
    }

    protected override void Recalculate()
    {
        float currentPartialPressure = o2.GetPartialPressure(airController.air);
        float targetPartialPressure = o2.GetPartialPressure(airController.targetAir);

        if (!(Mathf.Abs(targetPartialPressure - currentPartialPressure) > activateThreshold))
            return;

        float amountToPump = Mathf.Sign(targetPartialPressure - currentPartialPressure) * kgPerSec * deltaTime;
        amountToPump = amountToPump > tank.gasWeight ? tank.gasWeight : amountToPump; //clamp to the mass in tank
        float currentMass = o2.GetMass(airController.air);
        float newMass = currentMass + amountToPump;
        o2.SetMass(airController.air, newMass);
        tank.gasWeight -= amountToPump; //clamped by gasWeight property
    }
}

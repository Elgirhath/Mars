using Assets.Air.Gases;
using UnityEngine;

namespace Assets.Air.Pump
{
    public class NPump : Pump
    {
        public float kgPerSec;
        public float activateThreshold;

        public Gas n2;
        private GasTank tank;

        private CapsuleAirController airController;

        protected override void OnPumpStart() {
            airController = GetComponentInParent<CapsuleAirController>();
            tank = GetComponentInChildren<GasTank>();
        }

        protected override void Recalculate() {
            Air currentAir = airController.air;
            Air targetAir = airController.targetAir;
        
            if (!(Mathf.Abs(targetAir.pressure - currentAir.pressure) > activateThreshold))
                return;
        
            float amount = Mathf.Sign(targetAir.pressure - currentAir.pressure) * kgPerSec * deltaTime;
            amount = amount > tank.gasWeight ? tank.gasWeight : amount;
            float currentMass = n2.GetMass(airController.air);
            float newMass = currentMass + amount;
            n2.SetMass(airController.air, newMass);
            tank.gasWeight -= amount;
        }
    }
}

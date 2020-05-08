using Assets.Air.Gases;
using UnityEngine;

namespace Assets.Air.Pump
{
    public class CO2Pump : Pump {
        public float kgPerSec;
        public float activateThreshold;

        public Gas co2;

        private CapsuleAirController airController;
    
        protected override void OnPumpStart() {
            airController = GetComponentInParent<CapsuleAirController>();
        }

        protected override void Recalculate() {
            float currentMass = co2.GetMass(airController.air);
            float targetMass = co2.GetMass(airController.targetAir);
        
            if (Mathf.Abs(currentMass - targetMass) < activateThreshold)
                return;
        
            float maxAmount = targetMass - currentMass;
            float amount = Mathf.Sign(maxAmount) * kgPerSec * deltaTime;
            float amountClamped = Mathf.Clamp(amount, -Mathf.Abs(maxAmount), Mathf.Abs(maxAmount));
            float newMass = currentMass + amountClamped;
            co2.SetMass(airController.air, newMass);
        }
    }
}
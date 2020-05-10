using UnityEngine;

namespace Scripts.Player.Health.Condition
{
    public class Breathlessness : MedicalCondition
    {
        public float breathLeft;
        private EnergyController energyController;

        protected override void Start()
        {
            energyController = Player.instance.GetComponent<EnergyController>();
            breathLeft = 100f;
        }

        protected override void Update()
        {
            energyController.energy -= 20.0f * Time.deltaTime;
            breathLeft -= 10f * Time.deltaTime;
        }
    }
}
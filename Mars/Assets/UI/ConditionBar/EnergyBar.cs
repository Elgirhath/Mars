using Assets.Scripts.Player;
using Assets.Scripts.Player.Condition;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.ConditionBar
{
    public class EnergyBar : MonoBehaviour {
        private float longTermEnergyBarLengthLimit;
        private float currentLongTermEnergyBarLength;
        private RectTransform longTermEnergyBar;
    
        private Slider slider;
        private Image fillImage;

        public Color criticalValueColor;
        public Color semiCriticalValueColor;
        public Color defaultColor;

        private EnergyController energyController;

        //Critical Values
        [Range(0.0f, 1.0f)]
        public float criticalValue;
        [Range(0.0f, 1.0f)]
        public float semiCriticalValue;

        private void Start()
        {
            slider = GetComponentInChildren<Slider>();
            fillImage = transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
            longTermEnergyBar = gameObject.GetComponent<RectTransform>();
            longTermEnergyBarLengthLimit = longTermEnergyBar.rect.width;

            energyController = Player.instance.GetComponent<EnergyController>();
        }

        private void Update()
        {
            SetEnergyBar(energyController.energy / energyController.maxEnergy);
            SetLongTermEnergyBar(energyController.longTermEnergy / energyController.maxEnergy);
        }

        private void SetEnergyBar(float percentageValue)
        {
            slider.value = percentageValue;

            if (percentageValue < criticalValue)
            {
                fillImage.color = criticalValueColor;
            }
            else if (percentageValue < semiCriticalValue)
            {
                fillImage.color = semiCriticalValueColor;
            }
            else
            {
                fillImage.color = defaultColor;
            }
        }

        private void SetLongTermEnergyBar(float percentageValue)
        {
            longTermEnergyBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percentageValue * longTermEnergyBarLengthLimit);
        }
    }
}

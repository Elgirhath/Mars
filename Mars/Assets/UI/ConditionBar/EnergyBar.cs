using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.ConditionBar
{
    public class EnergyBar : MonoBehaviour {
        private float maxEnergyBarLengthLimit;
        private float currentMaxEnergyBarLength;
        private RectTransform maxEnergyBar;
    
        private Slider slider;
        private Image fillImage;
    
        public static EnergyBar instance;
    
        public Color criticalValueColor;
        public Color semiCriticalValueColor;
        public Color defaultColor;
        //Critical Values
        [Range(0.0f, 1.0f)]
        public float criticalValue;
        [Range(0.0f, 1.0f)]
        public float semiCriticalValue;

        private void Awake() {
            if (!instance)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            slider = GetComponentInChildren<Slider>();
        }

        private void Start() {
            fillImage = transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
            maxEnergyBar = gameObject.GetComponent<RectTransform>();
            maxEnergyBarLengthLimit = maxEnergyBar.rect.width;
        }

        public void ChangeEnergyBar(float percentageValue)
        {
            slider.value = percentageValue;
            if (percentageValue < semiCriticalValue)
            {
                fillImage.color = percentageValue < criticalValue ? criticalValueColor : semiCriticalValueColor;
            }
            else
            {
                fillImage.color = defaultColor;
            }
        }

        public void ChangeMaxEnergyBar(float percentageValue)
        {
            maxEnergyBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percentageValue * maxEnergyBarLengthLimit);
        }
    }
}

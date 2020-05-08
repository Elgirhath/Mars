using Assets.Scripts.Player.Condition;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.ConditionBar
{
    public class ThirstBar : MonoBehaviour {
        public int maxValue;
        public int value;

        private Text conditionText;
        private Image slider;
        private Image emptyBar;
    
        private ThirstController thirstController;

        public Color criticalValueColor;
        public Color emptyBarCriticalValueColor;
        public Color defaultColor;
        public Color emptyBarDefaultColor;
        //Critical Values
        [Range(0.0f, 1.0f)]
        public float criticalValue;

        private void Awake() {
            conditionText = GetComponentInChildren<Text>();
            slider = transform.Find("Fill").GetComponent<Image>();
            emptyBar = transform.Find("Empty").GetComponent<Image>();
        }

        private void Start() {
            thirstController = ThirstController.instance;

            maxValue = thirstController.maxThirst;
            value = (int) thirstController.thirst;
        }

        private void Update() {
            UpdateBar();
        }

        public void UpdateBar() {
            value = (int) thirstController.thirst;
            float percentageValue = (float) value / maxValue;
            conditionText.text = value + "/" + maxValue;
            slider.fillAmount = percentageValue;
        
            if (percentageValue < criticalValue)
                SetCriticalState();
            else
                SetDefaultState();
        }

        private void SetCriticalState() {
            slider.color = criticalValueColor;
            emptyBar.color = emptyBarCriticalValueColor;
        }

        private void SetDefaultState() {
            slider.color = defaultColor;
            emptyBar.color = emptyBarDefaultColor;
        }
    }
}

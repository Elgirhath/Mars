using Assets.Scripts.Player.Condition;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.ConditionBar
{
    public class HungerBar : MonoBehaviour {
        public int maxValue;
        public int value;

        private Text conditionText;
        private Image slider;
        private Image emptyBar;

        private HungerController hungerController;
        private DebuffsController debuffsController;
        private GameObject hungerDebuff;
    
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
            hungerController = HungerController.instance;

            maxValue = hungerController.maxHunger;
            value = (int) hungerController.hunger;
            debuffsController = DebuffsController.instance;
            hungerDebuff = debuffsController.transform.Find("Hunger").gameObject;
            hungerDebuff.SetActive(false);
        }

        private void Update() {
            value = (int) hungerController.hunger;
            float percentageValue = (float) value / maxValue;
            conditionText.text = value + "/" + maxValue;
            slider.fillAmount = percentageValue;
            if (percentageValue < criticalValue)
            {
                slider.color = criticalValueColor;
                emptyBar.color = emptyBarCriticalValueColor;
                if (!hungerDebuff.activeSelf)
                {
                    debuffsController.AddDebuff(hungerDebuff, "You are starving!");
                }
            }
            else
            {
                slider.color = defaultColor;
                emptyBar.color = emptyBarDefaultColor;
                if (hungerDebuff.activeSelf)
                {
                    debuffsController.RemoveDebuff(hungerDebuff);
                }
            }
        }
    }
}

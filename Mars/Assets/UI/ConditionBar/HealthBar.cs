using Scripts.Player.Health;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ConditionBar
{
    public class HealthBar : MonoBehaviour {
        public int maxValue;
        public int value;

        private Text conditionText;
        private Slider slider;

        private HealthController healthController;
    
        public static HealthBar instance;

        private void Awake() {
            if (!instance)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        
            conditionText = GetComponentInChildren<Text>();
            slider = GetComponentInChildren<Slider>();
        }

        private void Start() {
            healthController = HealthController.instance;

            maxValue = healthController.maxHealth;
            value = (int) healthController.health;
        }

        public void ChangeHealthBar()
        {
            value = (int) healthController.health;
            conditionText.text = value + "/" + maxValue;
            slider.value = (float)value / maxValue;
        }
    }
}

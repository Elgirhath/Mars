using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ConditionBar
{
    public class OxygenBar : MonoBehaviour {
        public int maxValue;
        public int value;
        private bool ultraCritical;
    
        public static OxygenBar instance;

        private Text conditionText;
        private Image oxygenIndicator;
        private Image oxygenBackground;
        private Outline conditionTextOutline;
        private Image ultraCriticalOutline;
    
        //Color of the bar when value under critical
        public Color criticalIndicatorColor;
    
        //Colors of conditionTextOutlines
        public Color outlineDefaultColor;
        public Color outlineCriticalColor;
    
        //Critical Values
        [Range(0.0f, 1.0f)]
        public float ultraCriticalValue;
        [Range(0.0f, 1.0f)]
        public float criticalValue;

        public float pulsingSpeed; //speed of UltraCriticalOutline pulsing
        private bool pulsing;
    
        private void Awake() {
            if (!instance)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            Transform oxygenContainer = transform.Find("OxygenFull");
            conditionText = oxygenContainer.GetComponentInChildren<Text>();
            conditionTextOutline = oxygenContainer.GetComponentInChildren<Outline>();
            oxygenIndicator = oxygenContainer.GetComponent<Image>();
            oxygenBackground = transform.Find("OxygenOut").GetComponent<Image>();
            ultraCriticalOutline = transform.Find("UltraCriticalOutline").GetComponent<Image>();
            ultraCriticalOutline.canvasRenderer.SetAlpha( 0.0f );
            ultraCriticalOutline.gameObject.SetActive(false);
        }

        private void Start() 
        {
            ultraCritical = false;
            pulsing = false;
        }

        public void ChangeOxygenBar(int oxygen) {
            value = oxygen;
            float percentageValue = (float)value / maxValue;
            conditionText.text = (int)(percentageValue*100) + "%";
            oxygenIndicator.fillAmount = percentageValue;
            if (percentageValue < criticalValue)
            {
                oxygenIndicator.color = criticalIndicatorColor;
                oxygenBackground.color = criticalIndicatorColor;
                conditionTextOutline.effectColor = outlineCriticalColor;
            }
            else
            {
                oxygenIndicator.color = Color.white;
                oxygenBackground.color = Color.white;
                conditionTextOutline.effectColor = outlineDefaultColor;
            }
            ultraCritical = percentageValue < ultraCriticalValue;
        }

        private void Update()
        {
            if (ultraCritical)
            {
                ultraCriticalOutline.gameObject.SetActive(true);
                if(!pulsing)
                    StartCoroutine(OutlinePulse());
            }
            else
            {
                StopCoroutine(OutlinePulse());
                pulsing = false;
                ultraCriticalOutline.canvasRenderer.SetAlpha( 0.0f );
                ultraCriticalOutline.gameObject.SetActive(false);
            }
        }
    
        private IEnumerator OutlinePulse()
        {
            pulsing = true;
            ultraCriticalOutline.CrossFadeAlpha(1,pulsingSpeed,true);
            yield return new WaitForSeconds(pulsingSpeed);
            ultraCriticalOutline.CrossFadeAlpha(0,pulsingSpeed,true);
            yield return new WaitForSeconds(pulsingSpeed);
            StartCoroutine(OutlinePulse());
        }
    
    }
}

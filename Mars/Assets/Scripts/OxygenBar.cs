using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour {
    public int maxValue;
    public int value;

    private Text conditionText;
    private Slider slider;

    public static OxygenBar instance;

    private void Awake() {
	    if (!instance)
		    instance = this;
	    else if (instance != this)
		    Destroy(gameObject);
	    
        conditionText = GetComponentInChildren<Text>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() 
    {
	    
    }

    public void ChangeOxygenBar(int oxygen) {
            value = oxygen;
            conditionText.text = value + "/" + maxValue;
            slider.value = (float)value / maxValue;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {
    public int maxValue;
    public int value;
    
    private Slider slider;
    private Image fillImage;

    private EnergyController energyController;
    
    public static EnergyBar instance;
    
    public Color criticalValueColor;
    public Color defaultColor;
    //Critical Values
    [Range(0.0f, 1.0f)]
    public float criticalValue;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        energyController = EnergyController.instance;

        maxValue = energyController.maxEnergy;
        value = (int) energyController.energy;
        fillImage = transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
    }

    public void ChangeEnergyBar()
    {
        value = (int) energyController.energy;
        float percentageValue = (float)value / maxValue;
        slider.value = percentageValue;
        fillImage.color = percentageValue < criticalValue ? criticalValueColor : defaultColor;
    }
}

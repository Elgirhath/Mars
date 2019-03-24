using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollector : MonoBehaviour, Interactable {
	[Tooltip("The ratio of how much water can be collected from a source with specified amount." +
	         "0 means no water can be collected, 1 means whole water from deposit can be collected.")]
	public float availableCapacityRatio;
	
	[Tooltip("Amount allowing the collector to run at its max speed.")]
	public float optimalAmount;

	public float maxLitresPerSecond;
	public float tankVolume;
	public float tankFill;
	
	private WaterSourceController sourceCtrl;

	private void Start() {
		sourceCtrl = WaterSourceController.instance;
	}

	public string tooltipText {
		get => "Press [E] to collect water";
	}

	public void Interact() {
		Debug.Log("Current fill: " + tankFill);
	}

	private void Update() {
		PullWater();
	}

	private void PullWater() {
		float pullAmount = sourceCtrl.GetPullAmount(this);
		pullAmount = Mathf.Clamp(pullAmount, 0f, tankVolume - tankFill);
		sourceCtrl.PullWater(transform.position, pullAmount);
		tankFill += pullAmount;
	}

	public float GetAvailableCapacity(float capacity) {
		return availableCapacityRatio * capacity;
	}

	public float GetTotalAvailableAmount(WaterSource source) {
		float availCap = GetAvailableCapacity(source.capacity);
		float usedAmount = source.GetUsedAmount();
		return availCap - usedAmount;
	}

	public float GetAvailableAmount(WaterSource source) {
		float amount = source.GetIntensityInPoint(transform.position) * GetTotalAvailableAmount(source);
		return amount;
	}
}

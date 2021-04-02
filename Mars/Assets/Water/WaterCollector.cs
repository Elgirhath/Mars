using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollector : MonoBehaviour {
	[Tooltip("The ratio of how much water can be collected from a source with specified amount." +
	         "0 means no water can be collected, 1 means whole water from deposit can be collected.")]
	public float availableCapacityRatio;
	
	[Tooltip("Amount allowing the collector to run at its max speed.")]
	public float optimalAmount;

	public float maxLitresPerSecond;
	public float tankVolume;
	public float tankFill;
	
	private WaterSourceController sourceCtrl;
	private PowerSocket powerSocket;
	private Inventory inventory;

	private void Start() {
		sourceCtrl = WaterSourceController.instance;
		powerSocket = GetComponent<PowerSocket>();
		inventory = Inventory.instance;
	}

	public void Interact() {
		FillCanteen();
	}

	private void FillCanteen() {
		Canteen canteen = inventory.FindItemsOfType<Canteen>()?[0];
		if (canteen == null) { // no canteen in inventory
			Debug.Log("No canteen in the inventory"); //TODO: display message
			return;
		}

		float collectedAmount = canteen.onDrinkAmount;
		float limit = Mathf.Min(tankFill, canteen.capacity - canteen.currentAmount);
		collectedAmount = Mathf.Clamp(collectedAmount, 0f, limit);
		canteen.currentAmount += collectedAmount;
		tankFill -= collectedAmount;
	}

	private void Update() {
		if (powerSocket.isPowered)
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

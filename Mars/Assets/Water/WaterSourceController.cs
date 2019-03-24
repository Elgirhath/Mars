using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSourceController : MonoBehaviour {
	public float minWaterAmount;
	public float maxWaterAmount;

	public static WaterSourceController instance;
	
	private WaterSource[] sources;

	private void Awake() {
		sources = GetComponentsInChildren<WaterSource>();

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		
	}

	public float GetAvailableAmount(WaterCollector collector) {
		float sum = 0f;
		foreach (var source in sources) {
			sum += collector.GetAvailableAmount(source);
		}

		return sum;
	}

	public float PullWater(WaterCollector collector) {
		float totalAvailableAmount = GetAvailableAmount(collector);
		if (totalAvailableAmount <= 0f)
			return 0f;

		float pullOptimizationRatio = totalAvailableAmount / collector.optimalAmount;
		float pulledAmount = collector.maxLitresPerSecond * Time.deltaTime * pullOptimizationRatio; //TODO: Add every X frames

		foreach (var source in sources) {
			float amount = collector.GetAvailableAmount(source);
			Debug.Log("amount: " + amount);
			float ratio = amount / totalAvailableAmount;
			source.amount -= pulledAmount * ratio;
		}

		return pulledAmount;
	}
}
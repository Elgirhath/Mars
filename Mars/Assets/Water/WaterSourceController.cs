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

	private void Start() {
		GenerateSources();
	}

	private void GenerateSources() {
		float total = Random.Range(minWaterAmount, maxWaterAmount);
		
		float sum = 0f;
		foreach (var source in sources) {
			source.amount = Random.Range(0f, 1f);
			sum += source.amount;
		}

		foreach (var source in sources) {
			source.amount *= sum.Equals(0f) ? 0f : (total / sum);
			source.capacity = source.amount;
		}
	}
	
	public float GetMultiSourceAmount(Vector3 position) {
		float sum = 0f;
		foreach (var source in sources) {
			sum += source.GetTotalAmount(position);
		}

		return sum;
	}

	public float GetMultiSourceAvailableAmount(WaterCollector collector) {
		float sum = 0f;
		foreach (var source in sources) {
			sum += collector.GetAvailableAmount(source);
		}

		return sum;
	}

	public void PullWater(Vector3 position, float pulledAmount) {
		float totalAvailableAmount = GetMultiSourceAmount(position);

		foreach (var source in sources) {
			float amount = source.GetTotalAmount(position);
			float ratio = totalAvailableAmount <= 0f ? 0f : amount / totalAvailableAmount;
			float newAmount = source.amount - pulledAmount * ratio;
			source.amount = Mathf.Clamp(newAmount, 0f, source.capacity);
		}
	}

	public float GetPullAmount(WaterCollector collector) {
		float totalAvailableAmount = GetMultiSourceAvailableAmount(collector);
		if (totalAvailableAmount <= 0f)
			return 0f;

		float pullOptimizationRatio = totalAvailableAmount / collector.optimalAmount;
		float pulledAmount = collector.maxLitresPerSecond * Time.deltaTime * pullOptimizationRatio; //TODO: Add every X frames

		return pulledAmount;
	}
}
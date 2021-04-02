using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class WaterSource : MonoBehaviour {
	private Vector3 position;
	[ReadOnly] public float capacity;
	[ReadOnly] public float amount;
	public float maxDistance;
	[HideInInspector]
	public AnimationCurve falloff = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);


	private PlayerController player;
	private void Start() {
		position = transform.position;
		player = PlayerController.instance;
	}

	public float GetFalloffFactor(float distance) {
		float partialDistance = distance / maxDistance;
		float factor = falloff.Evaluate(partialDistance);
		return factor;
	}

	public float GetIntensityInPoint(Vector3 point) {
		float dist = Vector3.Distance(position, point);
		float factor = GetFalloffFactor(dist);
		factor = Mathf.Clamp(factor, 0f, 1f);
		
		return factor;
	}

	public float GetTotalAmount(Vector3 collectorPosition) {
		float intensity = GetIntensityInPoint(collectorPosition);

		return amount * intensity;
	}

	public float GetUsedAmount() {
		return capacity - amount;
	}

	private void OnDrawGizmos() {
		Gizmos.color = new Color32(30, 30, 255, 100);
		Gizmos.DrawWireSphere(transform.position, 0.1f);
		Gizmos.DrawWireSphere(transform.position, maxDistance);
	}
}

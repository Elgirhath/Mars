using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class WaterDeposit : MonoBehaviour {
	private Vector3 position;
	public float amount;
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

	public float Collect(Vector3 collectorPosition) {
		float intensity = GetIntensityInPoint(collectorPosition);

		return amount * intensity;
	}
	

	private void Update() {
		Debug.Log("Distance: " + Vector3.Distance(transform.position, player.transform.position));
		Debug.Log("Intensity: " + GetIntensityInPoint(player.transform.position));
		Debug.Log("Collection: " + Collect(player.transform.position));
	}
}

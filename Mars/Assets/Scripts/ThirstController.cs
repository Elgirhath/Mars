using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstController : MonoBehaviour {
	public int maxThirst;
	public int initThirst;
	public ThirstBar thirstBar;

	private float thirst;
	public float GetThirst {
		get => thirst;
	}
	
	[Tooltip("Points per second")]
	public float dropSpeed;

	public static ThirstController instance;

	private void Awake() {
		if (!instance)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

	private void Start() {
		thirst = initThirst;
		thirstBar.maxValue = maxThirst;
		thirstBar.value = initThirst;
	}

	private void Update() {
		thirst -= dropSpeed * Time.deltaTime;
		thirstBar.value = (int)thirst;

		if (thirst <= 0.0f) {
			Debug.Log("U ded");
		}
	}
}

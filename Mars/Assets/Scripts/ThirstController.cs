using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstController : MonoBehaviour {
	public int maxThirst;
	public int initThirst;

	private float _thirst;
	public float thirst {
		get => _thirst;
		set => _thirst = Mathf.Clamp(value, 0, maxThirst);
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
		_thirst = initThirst;
	}

	private void Update() {
		_thirst -= dropSpeed * Time.deltaTime;

		if (_thirst <= 0.0f) {
			Debug.Log("U ded");
		}
	}
}

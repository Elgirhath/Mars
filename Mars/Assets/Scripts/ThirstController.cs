using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstController : MonoBehaviour {
	public int maxThirst;
	public int initThirst;
	//public float dehydrationDamage;

	private float _thirst;
	public float thirst {
		get => _thirst;
		set => _thirst = Mathf.Clamp(value, 0, maxThirst);
	}
	
	[Tooltip("Points per second")]
	public float dropSpeed;

	public static ThirstController instance;
	
	//private HealthController healthController;

	private void Awake() {
		if (!instance)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

	private void Start() {
		_thirst = initThirst;
		//healthController = HealthController.instance;
	}

	private void Update() {
		if(_thirst > 0.0f)
			_thirst -= dropSpeed * Time.deltaTime;
		else {
			Debug.Log("You died out of thirst!");
			//healthController.ChangeHealth(-dehydrationDamage * Time.deltaTime);
		}
	}
}

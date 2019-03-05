using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SphereController : MonoBehaviour, ItemController {
	[SerializeField]
	private Item _item;
	public Item item {
		get => _item;
		set => _item = value;
	}

	private void Start() {
		_item.onUse.AddListener(UseFunc);
	}

	private void UseFunc() {
		Debug.Log("You used " + name);
	}
}

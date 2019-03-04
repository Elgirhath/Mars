using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour, InventoryItem {
	[SerializeField]
	private ItemProperties properties;
	public ItemProperties Properties {
		get => properties;
		set => properties = value;
	}

	private void Start() {
		properties.onUse.AddListener(delegate { UseFunc(); });
	}

	private void UseFunc() {
		Debug.Log("You used " + name);
	}
}

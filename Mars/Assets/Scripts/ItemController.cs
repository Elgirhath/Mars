using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public interface ItemController {
	[SerializeField]
	Item item { get; set; }
}

[Serializable]
public class Item {
	public bool collectible;
	public string name;
	public Sprite sprite;
	public GameObject prefab;

	public ItemUseEvent onUse; //called when an item is used with LMB in inventory

	public Item() {}
	public Item(Item other) : this() {
		Set(other);
	}

	public void Set(Item other) {
		collectible = other.collectible;
		name = other.name;
		sprite = other.sprite;
		onUse = other.onUse;
		prefab = other.prefab;
	}
}

[Serializable]
public class ItemUseEvent : UnityEngine.UI.Button.ButtonClickedEvent {}
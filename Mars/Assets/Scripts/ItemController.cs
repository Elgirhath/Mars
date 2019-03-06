using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System.Collections;

public interface ItemController {
	[SerializeField]
	Item item { get; set; }
}

[CreateAssetMenu(fileName = "NewItem", menuName="Item", order = 81)]
public class Item : ScriptableObject {
	public bool collectible;
	public string name;
	public Sprite sprite;
	public GameObject prefab;
	public int stackLimit;

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
		stackLimit = other.stackLimit;
	}
}

[Serializable]
public class ItemUseEvent : UnityEngine.UI.Button.ButtonClickedEvent {}
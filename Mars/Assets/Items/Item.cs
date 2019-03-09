using System;
using UnityEngine;

public abstract class Item : ScriptableObject {
	[Space(10)]
	[Header("Item properties")]
	public bool collectible;
	public string itemName;
	public Sprite sprite;
	public GameObject prefab;
	public int stackLimit;

	public Item() {}
	public Item(Item other) : this() {
		Set(other);
	}

	public void Set(Item other) {
		collectible = other.collectible;
		itemName = other.itemName;
		sprite = other.sprite;
		prefab = other.prefab;
		stackLimit = other.stackLimit;
	}

	public abstract void Use();
}
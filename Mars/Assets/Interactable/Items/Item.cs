using System;
using UnityEngine;

public abstract class Item : ScriptableObject {
	[Space(10)]
	[Header("Item properties")]
	public bool collectible;
	public string itemName;
	public Sprite sprite;
	public GameObject prefab;
	public uint stackLimit;
	
	public string tooltip {
		get => "Press [E] to collect " + itemName;
	}

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

	public abstract void Use(ItemSlot slot);

	public GameObject Instantiate(Vector3 pos) {
		GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
		ItemHandler itemHandler = obj.GetComponent<ItemHandler>();
		if (itemHandler == null) {
			itemHandler = obj.AddComponent<ItemHandler>();
			itemHandler.item = this;
		} else if (itemHandler.item == null)
			itemHandler.item = this;
		
		return obj;
	}

	public void Drop() {
		ItemDropdownController.instance.Drop(this);
	}
}
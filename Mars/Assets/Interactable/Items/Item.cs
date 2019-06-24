using System;
using UnityEngine;

public abstract class Item : ScriptableObject {
	/*
	 * Scriptable object defining Item type, which can be added to the item handler
	 */
	[Space(10)]
	[Header("Item properties")]
	public bool createInstances = false;
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
		createInstances = other.createInstances;
		itemName = other.itemName;
		sprite = other.sprite;
		prefab = other.prefab;
		stackLimit = other.stackLimit;
	}

	public abstract void Use(ItemSlot slot);

	public GameObject Instantiate(Vector3 pos) {
		/*
		 * Instantiate object of the Item type at the position
		 */
		
		GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
		ItemHandler itemHandler = obj.GetComponent<ItemHandler>();
		if (itemHandler == null) { // Make sure the instantiated prefab contains ItemHandler (so that it could be added to inventory)
			itemHandler = obj.AddComponent<ItemHandler>();
			itemHandler.item = this;
		} else if (itemHandler.item == null)
			itemHandler.item = this;
		
		return obj;
	}

	public void Drop() {
		/*
		 * Drop from the inventory
		 */
		
		ItemDropdownController.instance.Drop(this);
	}
}
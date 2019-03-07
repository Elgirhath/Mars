using System;
using UnityEngine;

public interface ItemController {
	[SerializeField]
	Item item { get; set; }
}

[CreateAssetMenu(fileName = "NewItem", menuName="Item", order = 81)]
public class Item : ScriptableObject {
	public bool collectible;
	public string itemName;
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
		itemName = other.itemName;
		sprite = other.sprite;
		onUse = other.onUse;
		prefab = other.prefab;
		stackLimit = other.stackLimit;
	}
}

[Serializable]
public class ItemUseEvent : UnityEngine.UI.Button.ButtonClickedEvent {}
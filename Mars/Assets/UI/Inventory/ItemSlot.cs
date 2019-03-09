using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
	private Item _item = null;
	public Item item {
		get => _item;
		set {
			AddItem(value);
		}
	}
	private Text amountText;
	private Button button;
	private InventoryButton invButton;

	private uint _amount = 0;

	public uint amount {
		get => _amount;
		set {
			_amount = value;
			string amountString = _amount < 2 ? "" : _amount.ToString();
			amountText.text = amountString;
		}
	}

	private void Awake() {
		button = GetComponentInChildren<Button>();
		invButton = GetComponentInChildren<InventoryButton>();
		amountText = GetComponentInChildren<Text>();

		Disable();
	}

	public void AddItem(Item item) {
		Enable();
		
		if (item == this.item) {
			AddItemUnit();

			return;
		}
		button.gameObject.GetComponent<Image>().sprite = item.sprite;
		invButton = button.gameObject.GetComponent<InventoryButton>();
		UnsubscribeEvents();

		_item = item;
		amount = 1;

		SubscribeEvents();
	}

	public void RemoveItem() {
		UnsubscribeEvents();
		_item = null;
		button.gameObject.GetComponent<Image>().sprite = null;
		
		Disable();
	}

	public void SubscribeEvents() {
		invButton.onLeftClick += item.Use;
		invButton.onLeftClick += RemoveItemUnit;
		invButton.onRightClick += item.Drop;
		invButton.onRightClick += RemoveItemUnit;
	}

	public void UnsubscribeEvents() {
		try {
			invButton.onLeftClick -= item.Use;
			invButton.onLeftClick -= RemoveItemUnit;
			invButton.onRightClick -= item.Drop;
			invButton.onRightClick -= RemoveItemUnit;
		}
		catch {}
	}
	
	public void AddItemUnit() {
		if (amount >= item.stackLimit)
			throw new Exception();
		amount++;
	}

	public void RemoveItemUnit() {
		amount--;
		if (amount < 1)
			RemoveItem();
	}

	public void Enable() {
		foreach (Transform child in transform) {
			child.gameObject.SetActive(true);
		}
	}

	public void Disable() {
		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);
		}
	}
}
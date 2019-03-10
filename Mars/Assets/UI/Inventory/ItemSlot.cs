using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
	public GameObject buttonPrefab;
	
	private Text amountText;
	private Button button;
	private ClickHandler clickHandler;
	
	private Item _item = null;
	public Item item {
		get => _item;
		set => SetItem(value);
	}
	
	private uint _amount = 0;

	public uint amount {
		get => _amount;
		set => SetAmount(value);
	}
	
	private void SetItem(Item item) {
		if (item == null) {
			RemoveItem();
			return;
		}
		
		if (this.item)
			RemoveItem();

		button = Instantiate(buttonPrefab, transform).GetComponent<Button>();
		amountText = GetComponentInChildren<Text>();
		clickHandler = GetComponentInChildren<ClickHandler>();
		
		button.GetComponent<Image>().sprite = item.sprite;
		UnsubscribeEvents();

		_item = item;
		amount = 1;

		SubscribeEvents();	
	}

	private void SetAmount(uint amount) {
		if (amount > item.stackLimit)
			throw new Exception("No space on stack");
		_amount = amount;
		if (_amount < 1) {
			item = null;
		}
		else {
			string amountString = _amount < 2 ? "" : _amount.ToString();
			amountText.text = amountString;
		}
	}

	private void RemoveItem() {
		if (_item) {
			UnsubscribeEvents();
			foreach (Transform child in transform)
				Destroy(child.gameObject);
		}
		button = null;
		clickHandler = null;
		amountText = null;
		_item = null;
	}

	public void SubscribeEvents() {
		clickHandler.onLeftClick += item.Use;
		clickHandler.onLeftClick += RemoveItemUnit;
		clickHandler.onRightClick += item.Drop;
		clickHandler.onRightClick += RemoveItemUnit;
	}

	public void UnsubscribeEvents() {
		try {
			clickHandler.onLeftClick -= item.Use;
			clickHandler.onLeftClick -= RemoveItemUnit;
			clickHandler.onRightClick -= item.Drop;
			clickHandler.onRightClick -= RemoveItemUnit;
		}
		catch {}
	}

	public void RemoveItemUnit() {
		amount--;
	}
}
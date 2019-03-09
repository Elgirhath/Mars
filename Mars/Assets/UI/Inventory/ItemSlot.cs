using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
	public GameObject buttonPrefab;
	
	private Item _item = null;
	public Item item {
		get => _item;
		set {
			if (!value) {
				UnsubscribeEvents();
				_item = null;
			}
			else
				AddItem(value);
		}
	}
	private Text amountText;
	private Button button;
	private ClickHandler clickHandler;

	private uint _amount = 0;

	public uint amount {
		get => _amount;
		set {
			_amount = value;
			string amountString = _amount < 2 ? "" : _amount.ToString();
			amountText.text = amountString;
		}
	}

	public void AddItem(Item item) {
		if (item == this.item) {
			AddItemUnit();

			return;
		}

		button = Instantiate(buttonPrefab, transform).GetComponent<Button>();
		amountText = GetComponentInChildren<Text>();
		clickHandler = GetComponentInChildren<ClickHandler>();
		
		button.GetComponent<Image>().sprite = item.sprite;
		UnsubscribeEvents();

		_item = item;
		amount = 1;

		Debug.Log("A");
		SubscribeEvents();
	}

	public void RemoveItem() {
		item = null;
		Destroy(button.gameObject);
	}

	public void SubscribeEvents() {
		Debug.Log(clickHandler.ToString());
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
}
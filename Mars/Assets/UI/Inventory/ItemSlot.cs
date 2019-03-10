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
				Debug.Log("Set item = null");
				if (_item) {
					UnsubscribeEvents();
					foreach (Transform child in transform)
						Destroy(child.gameObject);
					button = null;
					clickHandler = null;
					amountText = null;
				}
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
			if (_amount < 1) {
				item = null;
			}
			else {
				string amountString = _amount < 2 ? "" : _amount.ToString();
				amountText.text = amountString;
			}
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

		SubscribeEvents();
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
	
	public void AddItemUnit() {
		if (amount >= item.stackLimit)
			throw new Exception();
		amount++;
	}

	public void RemoveItemUnit() {
		amount--;
		if (amount < 1)
			item = null;
	}
}
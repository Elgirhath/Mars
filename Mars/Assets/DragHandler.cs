﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	private Canvas canvas;
	private RectTransform rectTransform;
	private ItemSlot[] slots;
	private Inventory inventory;
	
	private Item item;
	private uint amount;

	void Awake() {
		canvas = gameObject.GetComponentInParent<Canvas>();
		rectTransform = GetComponent<RectTransform>();
	}

	private void Start() {
		inventory = Inventory.instance;
		slots = inventory.GetComponentsInChildren<ItemSlot>();
	}

	public void OnBeginDrag(PointerEventData eventData) {
		ItemSlot parent = GetComponentInParent<ItemSlot>();
		item = parent.item;
		amount = parent.amount;
		transform.SetParent(canvas.transform, true);
		parent.item = null;
	}

	public void OnDrag(PointerEventData eventData) {
		Vector3 mousePos = Input.mousePosition;
		rectTransform.position = mousePos;
	}

	public void OnEndDrag(PointerEventData eventData) {
		ItemSlot closestSlot = null;
		float minDist = 0;
		foreach (ItemSlot slot in slots) {
			float distance = Vector2.Distance(slot.transform.position, rectTransform.position);
			
			if (slot.item) continue;
			if (closestSlot && !(distance < minDist)) continue;
			
			closestSlot = slot;
			minDist = distance;
		}
		
		rectTransform.position = closestSlot.transform.position;
		Destroy(gameObject);
		
		closestSlot.item = item;
		closestSlot.amount = amount;
	}
}
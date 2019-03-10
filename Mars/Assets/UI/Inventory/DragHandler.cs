using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	private Canvas canvas;
	private RectTransform rectTransform;
	private ItemSlot[] slots;
	private ItemSlot origin;
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
		origin = GetComponentInParent<ItemSlot>();
		item = origin.item;
		amount = origin.amount;
		transform.SetParent(canvas.transform, true);
		origin.item = null;
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
			
			if (closestSlot && !(distance < minDist)) continue;
			
			closestSlot = slot;
			minDist = distance;
		}

		if (closestSlot.item) {
			uint leftAmount = amount;
			if (closestSlot.item == item) {
				uint newVal = closestSlot.amount + amount;
				closestSlot.amount = newVal > item.stackLimit ? item.stackLimit : newVal;
				leftAmount = newVal - item.stackLimit;
			}
			
			Debug.Log("Left: " + leftAmount);
			Destroy(gameObject);
			origin.item = item;
			origin.amount = leftAmount;
		}
		else {
			Destroy(gameObject);

			closestSlot.item = item;
			closestSlot.amount = amount;
		}
	}
}
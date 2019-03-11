using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
			if (closestSlot.item == item) { //merge 2 stacks
				uint newAmount = closestSlot.amount + amount;
				closestSlot.amount = newAmount > item.stackLimit ? item.stackLimit : newAmount;
				uint leftAmount = newAmount - closestSlot.amount;
				origin.item = item;
				origin.amount = leftAmount;
			}
			else { //swap
				origin.item = closestSlot.item;
				origin.amount = closestSlot.amount;
				closestSlot.item = item;
				closestSlot.amount = amount;
			}
			
			Destroy(gameObject);
			
		}
		else { //move
			Destroy(gameObject);

			closestSlot.item = item;
			closestSlot.amount = amount;
		}
	}
}
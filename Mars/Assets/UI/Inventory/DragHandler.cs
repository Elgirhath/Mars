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
			if (closestSlot.item == item) {
				uint leftAmount = amount;
				uint newAmount = closestSlot.amount + amount;
				Debug.Log("New amount: " + newAmount);
				closestSlot.amount = newAmount > item.stackLimit ? item.stackLimit : newAmount;
				leftAmount = newAmount - closestSlot.amount;
				origin.item = item;
				origin.amount = leftAmount;
			}
			else { //swap
				Debug.Log("Original amount: " + amount);
				Debug.Log("Dest amount: " + closestSlot.amount);
				origin.item = closestSlot.item;
				origin.amount = closestSlot.amount;
				closestSlot.item = null;
				
				closestSlot.item = item;
				closestSlot.amount = amount;
				Debug.Log(closestSlot.name);
				Debug.Log("Slot: " + closestSlot.name);
				Debug.Log("Amount: " + closestSlot.gameObject.GetComponentInChildren<Text>().text);
//				Debug.Break();
			}
			
			Destroy(gameObject);
			
		}
		else {
			Destroy(gameObject);

			closestSlot.item = item;
			closestSlot.amount = amount;
		}
	}
}
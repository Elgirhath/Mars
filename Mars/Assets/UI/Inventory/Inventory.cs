using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    private Transform panel;
    private ItemDropdownController dropdown;
    private ItemSlot[] slots;

    private ScrollingInfoController scrollingInfoController;

    public static Inventory instance;

    private void Awake() {
        if (!instance) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        dropdown = ItemDropdownController.instance;
        panel = transform.GetChild(0);
        scrollingInfoController = ScrollingInfoController.instance;

        slots = panel.GetComponentsInChildren<ItemSlot>();
    }

    public void AddItem(Item item) {
        bool isStacked = false;
        foreach (var slot in slots) { //check if can be stacked
            if (slot.item != item)
                continue;
            try {
                slot.amount++;
                isStacked = true;
            }
            catch{}
        }
        if (!isStacked) {
            foreach (var slot in slots) {
                if (slot.item) {
                    if (slot.item != item)
                        continue;
                    if (slot.amount >= item.stackLimit)
                        continue;
                }

                try {
                    slot.item = item; //if item can be added to the slot, add it. Else try to add to next slot
                    break;
                }
                catch {
                    //TODO: correct message that there is no space in backpack and pickup disabled
                    throw new Exception("No space in backpack");
                }
            }
        }
        
        scrollingInfoController.AddText(item.itemName, isItem: true);
    }
    
    public void Open() {
        foreach (Transform child in transform) { //enable all children
            child.gameObject.SetActive(true);
        }
    }

    public void Close() {
        foreach (Transform child in transform) { //disable all children
            child.gameObject.SetActive(false);
        }
    }
}
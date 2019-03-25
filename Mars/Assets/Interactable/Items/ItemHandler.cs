using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemHandler : MonoBehaviour, Interactable {
    
    public Item item;

    private void Start() {
        if (item.createInstances) {
            if (item.stackLimit != 1) {
                throw new Exception("Stack limit must be 1, when createInstances is set!");
            }
            Item copy = Instantiate(item, null);
            item = copy;
        }
    }

    public string tooltipText {
        get => item.tooltip;
    }

    public void Interact() {
        Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }
}
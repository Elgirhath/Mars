using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour, Interactable {
    public Item item;


    public string tooltipText {
        get => item.tooltip;
    }

    public void Interact() {
        Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }
}
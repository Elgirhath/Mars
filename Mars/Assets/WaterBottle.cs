using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : MonoBehaviour, ItemController {
    [SerializeField]
    private Item _item;
    public Item item {
        get => _item;
        set => _item = value;
    }

    private PlayerController playerController;

    private void Start() {
        _item.onUse.AddListener(Drink);
        playerController = PlayerController.instance;
    }

    private void Drink() {
        Debug.Log("You drunk a bottle of water!");
        playerController.GetComponent<ThirstController>().thirst += 20;
    }
}

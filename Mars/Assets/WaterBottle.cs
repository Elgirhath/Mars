using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WaterBottle", menuName="Item/Bottle of water")]
public class WaterBottle : Item {
    [Space(10)]
    [Header("Water bottle properties")]
    public int pointsOnDrink;
    
    private PlayerController playerController;

    private void Drink() {
        Debug.Log("You drunk a bottle of water!");
        playerController = PlayerController.instance;
        playerController.GetComponent<ThirstController>().thirst += pointsOnDrink;
    }

    public override void Use() {
        Drink();
    }
}

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
    private ThirstController thirstController;

    private void Drink() 
    {
        string info = "You drunk a bottle of water. (+" + pointsOnDrink + ")";
        GameObject.FindGameObjectWithTag("ScrollingInfo").GetComponent<ScrollingInfoController>().AddText(info, isItem: false);
        thirstController = ThirstController.instance;
        thirstController.thirst += pointsOnDrink;
    }

    public override void Use() {
        Drink();
    }
}

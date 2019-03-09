using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "FoodRations", menuName="Item/Food rations")]
public class FoodRations : Item {
    [Space(10)]
    [Header("Food rations properties")]
    public int pointsOnEat;
    
    private PlayerController playerController;
    private HungerController hungerController;

    private void Eat()
    {
        string info = "You ate some food rations. (+" + pointsOnEat + ")";
        GameObject.FindGameObjectWithTag("ScrollingInfo").GetComponent<ScrollingInfoController>().AddText(info, isItem: false);
        hungerController = HungerController.instance;
        hungerController.hunger += pointsOnEat;
    }

    public override void Use() {
        Eat();
    }
}

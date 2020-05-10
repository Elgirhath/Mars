using Scripts.Player;
using Scripts.Player.Health;
using UI.Inventory;
using UI.ScrollingInfo;
using UnityEngine;

namespace Prefabs.Interactable.Items.Food_rations
{
    [CreateAssetMenu(fileName = "FoodRations", menuName="Item/Food rations")]
    public class FoodRations : Item {
        [Space(10)]
        [Header("Food rations properties")]
        public int pointsOnEat;
    
        private Player player;
        private HungerController hungerController;

        private void Eat()
        {
            string info = "You ate some food rations. (+" + pointsOnEat + ")";
            ScrollingInfoController.instance.AddText(info, isItem: false);
            hungerController = HungerController.instance;
            hungerController.hunger += pointsOnEat;
        }

        public override void Use(ItemSlot slot) {
            Eat();
            slot.amount--;
        }
    }
}
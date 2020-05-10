using Scripts.Player;
using Scripts.Player.Health;
using UI.Inventory;
using UI.ScrollingInfo;
using UnityEngine;

namespace Prefabs.Interactable.Items.Bottle_of_water
{
    [CreateAssetMenu(fileName = "WaterBottle", menuName="Item/Bottle of water")]
    public class WaterBottle : Item {
        [Space(10)]
        [Header("Water bottle properties")]
        public int pointsOnDrink;
    
        private Player player;
        private ThirstController thirstController;

        private void Drink() 
        {
            string info = "You drunk a bottle of water. (+" + pointsOnDrink + ")";
            ScrollingInfoController.instance.AddText(info, isItem: false);
            thirstController = ThirstController.instance;
            thirstController.thirst += pointsOnDrink;
        }

        public override void Use(ItemSlot slot) {
            Drink();
            slot.amount--;
        }
    }
}

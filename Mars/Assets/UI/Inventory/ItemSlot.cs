using System;
using Assets.Prefabs.Interactable.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.Inventory
{
    public class ItemSlot : MonoBehaviour {
        /*
	 * Slot in the inventory, responsible for actions on the items
	 */
        public GameObject buttonPrefab;
	
        private Text amountText;
        private Button button;
        private ClickHandler clickHandler;
	
        private Item _item = null;
        public Item item {
            get => _item;
            set => SetItem(value);
        }
	
        private uint _amount = 0;

        public uint amount {
            get => _amount;
            set => SetAmount(value);
        }
	
        private void SetItem(Item item) {
            /*
		 * Sets item in this slot and the amount to 1
		 */
		
            if (item == null) { // If null is passed - remove item from this slot
                RemoveItem();
                return;
            }
		
            if (this.item) // If the slot already contains an item - remove it
                RemoveItem();

            button = Instantiate(buttonPrefab, transform).GetComponent<Button>();
            amountText = GetComponentInChildren<Text>();
            clickHandler = GetComponentInChildren<ClickHandler>();
		
            button.GetComponent<Image>().sprite = item.sprite;
            UnsubscribeEvents();

            _item = item;
            amount = 1;

            SubscribeEvents();	
        }

        private void SetAmount(uint amount) {
            /*
		 * Sets the item amount on this slot and updates the button text. Throws exception if the amount cannot be set
		 */
		
            if (amount > item.stackLimit)
                throw new Exception("No space on stack");
            _amount = amount;
            if (_amount < 1) {
                item = null;
            }
            else {
                string amountString = _amount < 2 ? "" : _amount.ToString();
                amountText.text = amountString;
            }
        }

        private void RemoveItem() {
            /*
		 * Remove the item from this slot completely
		 */
		
            if (_item) {
                UnsubscribeEvents();
                foreach (Transform child in transform) {
                    child.SetParent(null);
                    Destroy(child.gameObject);
                }
            }
            button = null;
            clickHandler = null;
            amountText = null;
            _item = null;
        }

        public void Use() {
            item.Use(this);
        }

        private void SubscribeEvents() {
            clickHandler.onLeftClick += Use;
            clickHandler.onRightClick += item.Drop;
            clickHandler.onRightClick += SubtractItem;
        }

        private void UnsubscribeEvents() {
            try {
                clickHandler.onLeftClick -= Use;
                clickHandler.onRightClick -= item.Drop;
                clickHandler.onRightClick -= SubtractItem;
            }
            catch {}
        }

        private void SubtractItem() {
            amount--;
        }
    }
}
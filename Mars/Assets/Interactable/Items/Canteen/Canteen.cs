using UnityEngine;

[CreateAssetMenu(fileName = "Canteen", menuName="Item/Canteen")]
public class Canteen : Item {
	[Space(10)]
	[Header("Canteen properties")]
	public float capacity;

	private float _currentAmount;
	public float currentAmount {
		get => _currentAmount;
		set => _currentAmount = Mathf.Clamp(value, 0f, capacity);
	}
	public float onDrinkAmount;

	private ThirstController thirstController;
	
	public override void Use(ItemSlot slot) {
		thirstController = ThirstController.instance;

		float limit = Mathf.Min(thirstController.maxThirst - thirstController.thirst, currentAmount);
		float drinkAmountClamped = Mathf.Clamp(onDrinkAmount, 0f, limit);
		thirstController.thirst += drinkAmountClamped;
		currentAmount -= drinkAmountClamped;
		
		string info = "You drunk water from canteen. (+" + drinkAmountClamped + ")";
		ScrollingInfoController.instance.AddText(info, isItem: false);
	}
}
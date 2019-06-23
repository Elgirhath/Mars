using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
	public string tooltipText;
	
	public InteractEvent onInteract;

	public void Interact() {
		onInteract.Invoke();
	}
}

[Serializable]
public class InteractEvent : UnityEvent {}
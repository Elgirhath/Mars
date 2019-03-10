using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable {
	string tooltipText { get; }
	void Interact();
}
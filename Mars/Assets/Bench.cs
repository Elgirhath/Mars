using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : MonoBehaviour, Interactable {
    public string tooltipText {
        get => "Press [E] to sit";
    }
    public void Interact() {
        Debug.Log("U sit on bench! Congrats!");
    }
}

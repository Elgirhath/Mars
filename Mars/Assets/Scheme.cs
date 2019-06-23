using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheme : MonoBehaviour {
    public GameObject origin;

    public void Apply() {
        Instantiate(origin, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public string tooltipText {
        get => "Press E to build";
    }
    public void Interact() {
        Apply();
    }
}
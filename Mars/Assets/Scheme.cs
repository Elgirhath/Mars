using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheme : MonoBehaviour {
    public GameObject origin;
    private Interactable interactable;

    private void Start() {
        interactable = gameObject.AddComponent<Interactable>();

        if (interactable.onInteract == null)
            interactable.onInteract = new InteractEvent();

        interactable.applyOutline = false;
        interactable.onInteract.AddListener(Interact);
        interactable.tooltipText = "Press E to build";

        CreateCollider();
    }

    public void Apply() {
        Instantiate(origin, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Interact() {
        Apply();
    }

    private void CreateCollider() {
        Collider collider = GetComponent<Collider>();
        if (collider == null) {
            collider = gameObject.AddComponent<Collider>();
        }

        collider.isTrigger = true;
        collider.enabled = true;
    }
}
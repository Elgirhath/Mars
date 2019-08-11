using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BuildMenuItem : MonoBehaviour {
    public Sprite sprite;
    public List<BuildMaterial> matsNeeded;
    private List<BuildMaterial> matsLeft;

    private GameObject origin;
    private Interactable interactable;

    private void Start() {
        matsLeft = matsNeeded.ConvertAll(mat => new BuildMaterial() { // Deep copy matsNeeded list
            item = mat.item,
            count = mat.count
        });
    }

    public void AcceptScheme(GameObject original) {
        /*
         * Accept the position of the scheme and start building
         */
        origin = original;

        if (matsNeeded.Count <= 0 || matsNeeded.Sum(mat => mat.count) <= 0) // No materials needed -> scheme should apply immediately
        {
            Finish();
            return;
        }

        interactable = gameObject.AddComponent<Interactable>();

        if (interactable.onInteract == null)
            interactable.onInteract = new InteractEvent();

        interactable.applyOutline = false;
        interactable.onInteract.AddListener(Interact);
        UpdateTooltip();

        CreateBuildUICollider();
    }

    public void UpdateTooltip() {
        /*
         * Updates the tooltip displaying the amount of materials needed and already put
         */
        
        StringBuilder sb = new StringBuilder();
        foreach (var matN in matsNeeded) {
            sb.Append(matN.item.name + ": ");
            BuildMaterial matL = BuildMaterial.GetByKey(matsLeft, matN.item);
            int matPut = matN.count - matL.count;

            sb.Append(matPut + "/" + matN.count + "\n");
        }

        interactable.tooltipText = sb.ToString();
    }

    private bool TryPutPart() {
        /*
         * Tries to put a part taken from inventory into the building. Returns true on success and false on failure.
         */
        
        Inventory inventory = Inventory.instance;
        foreach (var matN in matsNeeded) {
            BuildMaterial matL = BuildMaterial.GetByKey(matsLeft, matN.item);
            
            if (matL.count <= 0) // Check if this material has already been exhausted
                continue;
            
            if (inventory.Pull(matN.item)) { // Takes the item from the inventory
                matL.count--;
                return true;
            }
        }

        return false;
    }

    private void Finish() {
        /*
         * Instantiates the original prefab in place of the scheme
         */
        
        Instantiate(origin, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Interact() {
        TryPutPart();
        UpdateTooltip();

        if (IsFinished())
            Finish();
    }

    private bool IsFinished() {
        /*
         * Check if player has put every part into the building
         */
        
        foreach (var matN in matsNeeded) {
            BuildMaterial matL = BuildMaterial.GetByKey(matsLeft, matN.item);
            if (matL.count > 0)
                return false;
        }

        return true;
    }

    private void CreateBuildUICollider() {
        /*
         * Creates the trigger collider used for raycast, to allow player to interact with the scheme
         */
        
        Collider collider = GetComponent<Collider>();
        if (collider == null) {
            collider = gameObject.AddComponent<BoxCollider>();
        }

        collider.isTrigger = true;
        collider.enabled = true;
    }
}

[Serializable]
public class BuildMaterial {
    public Item item;
    public int count;

    public static BuildMaterial GetByKey(List<BuildMaterial> list, Item item) {
        foreach (var mat in list) {
            if (mat.item == item)
                return mat;
        }

        return null;
    }
}
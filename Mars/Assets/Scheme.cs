using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Scheme : MonoBehaviour {
    public GameObject origin;
    private Interactable interactable;
    public List<BuildMaterialTuple> matsNeeded;
    private List<BuildMaterialTuple> matsPut = new List<BuildMaterialTuple>();

    private void Start() {
        interactable = gameObject.AddComponent<Interactable>();

        if (interactable.onInteract == null)
            interactable.onInteract = new InteractEvent();

        interactable.applyOutline = false;
        interactable.onInteract.AddListener(Interact);
        interactable.tooltipText = GetTooltip();

        CreateCollider();
    }

    public string GetTooltip() {
        StringBuilder sb = new StringBuilder();
        foreach (var matN in matsNeeded) {
            sb.Append(matN.item.name + ": ");
            int countPut = 0;
            foreach (var matP in matsPut) {
                if (matN.item == matP.item) {
                    countPut = matP.count;
                    break;
                }
            }

            sb.Append(countPut + "/" + matN.count + "\n");
        }

        Debug.Log(sb.ToString());
        return sb.ToString();
    }

    private void PutPart() {
        Inventory inventory = Inventory.instance;
        foreach (var material in matsNeeded) {
            if (inventory.Pull(material.item)) {
                foreach (BuildMaterialTuple tuple in matsPut) {
                    if (tuple.item == material.item) {
                        tuple.count++;
                        return;
                    }
                }
                
                matsPut.Add(new BuildMaterialTuple() {
                    item = material.item,
                    count = 1
                });
                return;
            }
        }
    }

    private void Apply() {
        Instantiate(origin, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Interact() {
        PutPart();
        interactable.tooltipText = GetTooltip();
        foreach (var matN in matsNeeded) {
            bool flag = false;
            
            foreach (var matP in matsPut) {
                if (matN.item == matP.item && matN.count == matP.count) {
                    flag = true;
                    break;
                }
            }

            if (!flag)
                return;
        }

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
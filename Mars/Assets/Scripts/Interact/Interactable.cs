using System;
using System.Collections.Generic;
using Assets.UI.Tooltip;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interact
{
    public class Interactable : MonoBehaviour {

        [SerializeField]
        private string _tooltipText;
        public string tooltipText {
            get => _tooltipText;
            set {
                _tooltipText = value;
                if (isSelected)
                    Tooltip.instance.SetText(_tooltipText);
            }
        }
	
        public bool applyOutline = true;
        public bool deselectOnInteract;
	
        public InteractEvent onInteract;

        private MaterialTree originMaterialTree;
        private Tooltip tooltip;
        private Material selectedMaterial;
        private bool isSelected = false;
        private bool isInitialized = false;

        private void Start() {
            tooltip = Tooltip.instance;
            selectedMaterial = InteractController.instance.selectedMaterial;
            isInitialized = true;
        }

        public void Interact() {
            if (deselectOnInteract)
                Deselect();
		
            onInteract.Invoke();
        }
	
        public void Select() {
            if (!isInitialized)
                return;
		
            if (applyOutline) {
                originMaterialTree = new MaterialTree(gameObject);
                MaterialTree.ApplyOutline(gameObject, selectedMaterial);
            }

            isSelected = true;
            tooltip.OpenTooltip(transform, GetComponent<Interactable>().tooltipText);
        }

        public void Deselect() {
            if (applyOutline) {
                originMaterialTree.Apply(gameObject);
            }

            isSelected = false;
            tooltip.Disable();
        }
	
        private class MaterialTree { // Keeps MaterialTree for every object in the subtree
            private Material parentMaterial;
            private Dictionary<Transform, MaterialTree> childTrees = new Dictionary<Transform, MaterialTree>();
	
            public MaterialTree() {}
	
            public MaterialTree(GameObject obj) : this() {
                GetFromObject(obj);
            }
            public void GetFromObject(GameObject obj) {
                try {
                    parentMaterial = obj.GetComponent<Renderer>().material;
                }
                catch {}
			
                foreach (Transform child in obj.transform) {
                    MaterialTree subtree = new MaterialTree(child.gameObject);
                    childTrees.Add(child, subtree);
                }
            }
	
            public void Apply(GameObject obj) {
                try {
                    obj.GetComponent<Renderer>().material = parentMaterial;
                }
                catch {}

                foreach (Transform child in obj.transform) {
                    try {
                        childTrees[child].Apply(child.gameObject);
                    }
                    catch {}
                }
            }
	
            public static void Apply(GameObject obj, Material material) {
                try {
                    obj.GetComponent<Renderer>().material = material;
                }
                catch {}

                foreach (Transform child in obj.transform) {
                    Apply(child.gameObject, material);
                }
            }
		
            public static void ApplyOutline(GameObject obj, Material material) {
                try {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    Material oldMat = renderer.material;
                    Material newMat = new Material(material);
                    if (oldMat.HasProperty("_Color")) {
                        newMat.SetColor("_BaseColor", oldMat.color);
                    }

                    newMat.SetTexture("_MainTex", oldMat.mainTexture);
                    newMat.SetTexture("_Normal", oldMat.GetTexture("_NormalMap"));
//				newMat.SetTexture("_Metallic", oldMat.GetTexture("_Metallic"));
                    if (oldMat.HasProperty("_NormalScale")) {
                        newMat.SetFloat("_NormalScale", oldMat.GetFloat("_NormalScale"));
                    }

                    renderer.material = newMat;
                }
                catch {}

                foreach (Transform child in obj.transform) {
                    ApplyOutline(child.gameObject, material);
                }
            }
        }
    }

    [Serializable]
    public class InteractEvent : UnityEvent {}
}
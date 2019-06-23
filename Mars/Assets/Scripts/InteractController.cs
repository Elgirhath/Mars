using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

public class InteractController : MonoBehaviour {
	[Tooltip("Max distance from camera in which objects can be collected")]
	public float maxCollectDistance;
	[Tooltip("Weight from 0 to 1. 0 means distance only, 1 means angle only.")]
	[Range(0.0f, 1.0f)]
	public float weight;
	[Tooltip("Max angle from camera forward in which objects can be collected")]
	[Range(0.0f, 90.0f)]
	public float maxAngle;

	private Camera cam;
	private Transform camTransform;
	private bool allowInteract;
	private Collider[] nearItems;
	private GameObject target;

	private Tooltip tooltip;
			
	private Collider closestItem;
	private float closestItemFactor;

	private MaterialTree originMaterialTree;

	public Material selectedMaterial;

	void Start() {
		cam = GetComponentInChildren<Camera>();
		camTransform = cam.transform;
		target = null;
		
		tooltip = Tooltip.instance;
		
		closestItem = null;
		closestItemFactor = 0;
	}
	
	void Update()
	{
		allowInteract = false;
		closestItem = null;
		closestItemFactor = 0;
		
		if (!HitScan())
		{
			AreaScan();
		}
		
		if (closestItem) // if any item can be collected
		{
			allowInteract = true;
			if (closestItem.gameObject != target) { // if our current target isn't the one we want to pick up - change current target
				if(target)
					Deselect();
				target = closestItem.gameObject;
				Select();
			}
		}
		else if(target)
		{
			Deselect();
			target = null;
		}
		
		if (allowInteract) {
			if (Input.GetButtonDown("Use")) {
				target.GetComponent<Interactable>().Interact();
				Deselect();
			}
		}
	}
	
	
	private bool HitScan()
	{
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit)) {
			if (hit.distance <= maxCollectDistance * 0.85f)
			{
				if (hit.transform.gameObject.GetComponent<Interactable>() != null) {
					closestItem = hit.transform.gameObject.GetComponent<Collider>();
					return true;
				}
			}
		}

		return false;
	}

	private void AreaScan()
	{
		nearItems = Physics.OverlapSphere(transform.position, maxCollectDistance); //get all objects in range
		for (int i = 0; i < nearItems.Length; ++i) {
			Vector3 camItemVector = nearItems[i].transform.position - camTransform.position; //vector from camera to the object
			float angle = Vector3.Angle(camItemVector, camTransform.forward);
			if (angle > maxAngle)
				continue;
			
			if (nearItems[i].transform.gameObject.GetComponent<Interactable>() != null) {
				float factor = weight * camItemVector.magnitude + (1 - weight) * angle; //selecting the right object by the lowest factor
				if (factor < closestItemFactor || !closestItem)
				{
					closestItemFactor = factor;
					closestItem = nearItems[i];
				}
			}
		}
	}
	
	void Select() {
		originMaterialTree = new MaterialTree(target.gameObject);
		MaterialTree.ApplyOutline(target.gameObject, selectedMaterial);
		
		tooltip.OpenTooltip(target.transform, target.GetComponent<Interactable>().tooltipText);
	}

	void Deselect()
	{
		originMaterialTree.Apply(target.gameObject);
		
		tooltip.Disable();
	}
	
	private class MaterialTree { // Keeps ObjectState for every object in the subtree
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
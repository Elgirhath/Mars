using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

public class ItemCollectingController : MonoBehaviour {
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
	private bool allowCollecting;
	private Collider[] nearItems;
	private GameObject target;
	private Material targetMaterial;

	private Inventory inventory;

	private TooltipController tooltip;

	private Shader standardShader;
	private Shader outline;
	
	//Shader settings
	public Color firstOutlineColor = Color.white;
	public Color secondOutlineColor = Color.white;
	public float firstOutlineWidth;
	public float secondOutlineWidth;

	void Start() {
		cam = GetComponentInChildren<Camera>();
		camTransform = cam.transform;
		target = null;
		standardShader = Shader.Find("Standard");
		outline = Shader.Find("Outlined/UltimateOutline");
		
		tooltip = TooltipController.instance;
		inventory = Inventory.instance;
	}

	

	void Update()
	{
		Collider closestItem = null;
		float closestItemFactor = 0;
		
		allowCollecting = false;

		nearItems = Physics.OverlapSphere(transform.position, maxCollectDistance); //get all objects in range
		
		
		for (int i = 0; i < nearItems.Length; ++i) {
			Vector3 camItemVector = nearItems[i].transform.position - camTransform.position; //vector from camera to the object
			float angle = Vector3.Angle(camItemVector, camTransform.forward);
			if (angle > maxAngle)
				continue;
			try {
				Item properties = nearItems[i].transform.gameObject.GetComponent<ItemController>().item;
				if (properties.collectible) {
					float factor = weight * camItemVector.magnitude + (1 - weight) * angle; //selecting the right object by the lowest factor
					if (factor < closestItemFactor || !closestItem)
					{
						closestItemFactor = factor;
						closestItem = nearItems[i];
					}
				}
			}
			catch {}
		}

		if (closestItem) // if any item can be collected
		{
			allowCollecting = true;
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
		
		if (allowCollecting) {
			if (Input.GetButtonDown("Use")) {
				inventory.AddItem(target.GetComponent<ItemController>().item);
				Deselect();
				Destroy(target);
				Debug.Log("Added to inventory");
			}
		}
		
	}
	void Select()
	{
		targetMaterial = target.GetComponent<Renderer>().material;
		
		targetMaterial.shader = outline;
		targetMaterial.SetColor("_FirstOutlineColor", firstOutlineColor);
		targetMaterial.SetColor("_SecondOutlineColor", secondOutlineColor);
		targetMaterial.SetFloat("_FirstOutlineWidth", firstOutlineWidth);
		targetMaterial.SetFloat("_SecondOutlineWidth", secondOutlineWidth);
		
		tooltip.OpenPickupTooltip(target.transform, target.GetComponent<ItemController>().item.name);
	}

	void Deselect()
	{
		targetMaterial.shader = standardShader;
		tooltip.Disable();
	}
}
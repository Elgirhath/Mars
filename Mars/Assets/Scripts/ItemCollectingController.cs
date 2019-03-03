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
	private List<Tuple<Transform, float>> itemsInFront = new List<Tuple<Transform, float>>();
	private GameObject target;

	private InventoryController inventory;

	private GameObject tooltip;
	private RectTransform tooltipTransform;

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
		tooltip = GameObject.Find("Collect Tooltip");
		tooltip.SetActive(false);
		tooltipTransform = tooltip.GetComponent<RectTransform>();

		inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryController>();
	}

	void addTooltip()
	{
		Material targetMaterial = target.GetComponent<Renderer>().material;
		Vector3 itemPosition = cam.WorldToScreenPoint(target.transform.position);
		
		targetMaterial.shader = outline;
		targetMaterial.SetColor("_FirstOutlineColor", firstOutlineColor); // z jakiegos powodu dzialaja tylko built-in kolory...
		targetMaterial.SetVector("_SecondOutlineColor", secondOutlineColor);
		targetMaterial.SetFloat("_FirstOutlineWidth", firstOutlineWidth);
		targetMaterial.SetFloat("_SecondOutlineWidth", secondOutlineWidth);

		tooltipTransform.anchoredPosition = itemPosition;
		tooltip.SetActive(true);
	}

	void removeTooltip()
	{
		target.GetComponent<Renderer>().material.shader = standardShader;
		tooltip.SetActive(false);
	}

	void Update() {
		
		allowCollecting = false;
		itemsInFront.Clear();

		nearItems = Physics.OverlapSphere(transform.position, maxCollectDistance); //get all objects in range
		for (int i = 0; i < nearItems.Length; ++i) {
			Vector3 camItemVector = nearItems[i].transform.position - camTransform.position; //vector from camera to the object
			float angle = Vector3.Angle(camItemVector, camTransform.forward);
			if (angle > maxAngle)
				continue;
			try {
				if (nearItems[i].transform.gameObject.GetComponent<MultiTag>().Contains("Collectible")) {
					float factor = weight * camItemVector.magnitude + (1 - weight) * angle; //selecting the right object by the lowest factor
					itemsInFront.Add(new Tuple<Transform, float>(nearItems[i].transform, factor));
				}
			}
			catch {}
		}

		itemsInFront.Sort((a, b) => a.Item2.CompareTo(b.Item2));
		if (itemsInFront.Count > 0) {
			allowCollecting = true;
			if (target != null)
			{
				removeTooltip();
			}
			target = itemsInFront.First().Item1.gameObject;
			addTooltip();
		}
		else if (target != null)
		{
			removeTooltip();
		}
//		Debug.Log(itemsInFront.Count);

		//OLD HITCAST
		/*
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit)) {
			try {
				if (hit.transform.gameObject.GetComponent<MultiTag>().Contains("Collectible"))
				{
					if (hit.distance <= maxCollectDistance) {
						target = hit.transform.gameObject;
						allowCollecting = true;
					}
				}
			}
			catch {}
		}
		*/
		
		if (allowCollecting) {
			if (Input.GetButtonDown("Use")) {
				inventory.AddItem(target.GetComponent<InventoryItem>());
				removeTooltip();
				Destroy(target);
				Debug.Log("Added to inventory");
			}
		}
		
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemCollectingController : MonoBehaviour {
	public float maxCollectDistance;
	public float magnitudeWeight;
	public float angleWeight;
	public float maxAngle;

	private Camera cam;
	private bool allowCollecting;
	private Collider[] nearItems;
	private List<Tuple<Transform, float>> itemsInFront = new List<Tuple<Transform, float>>();
	private GameObject target;

	void Start() {
		cam = GetComponentInChildren<Camera>();
	}

	void Update() {
		allowCollecting = false;
		itemsInFront.Clear();
		
		nearItems = Physics.OverlapSphere(transform.position, maxCollectDistance);
		for (int i = 0; i < nearItems.Length; ++i)
		{
			Vector3 itemVector = nearItems[i].transform.position - transform.position;
			float angle = Vector3.Angle(itemVector, transform.forward);
			if (angle > maxAngle)
				continue;
			try
			{
				if (nearItems[i].transform.gameObject.GetComponent<MultiTag>().Contains("Collectible"))
				{
					itemsInFront.Add(new Tuple<Transform, float>(nearItems[i].transform,
						magnitudeWeight * itemVector.magnitude + angleWeight * angle));
				}
			}
			catch {}
		}
		itemsInFront.Sort((a, b) => a.Item2.CompareTo(b.Item2));
		if (itemsInFront.Count > 0)
		{
			allowCollecting = true;
			target = itemsInFront.First().Item1.gameObject;
		}
		Debug.Log(itemsInFront.Count);

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
				Destroy(target);
				Debug.Log("Dodano do ekwipunku");
			}
		}
		
	}
}
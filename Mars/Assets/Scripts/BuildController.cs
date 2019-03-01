using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BuildController : MonoBehaviour {
	public float maxDistance;
	public float rotLerpSpeed;
	public float rotationSpeed;
	
	public Material schemeMaterial;

	private Transform cam;
	private bool isPlacing;
	private GameObject scheme;
	private ObjectTreeState savedState;
	
	private float schemeYRotation;
	private Vector3 placePosition;
	private int schemeLayer;
	
	void Start() {
		isPlacing = false;
		cam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform;
		placePosition = Vector3.zero;
		schemeLayer = LayerMask.NameToLayer("Ignore Raycast");
		schemeYRotation = 0.0f;
	}

	void Update() {
		bool wasHit = Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo);

		bool isInRange = Vector3.Distance(hitInfo.point, cam.position) < maxDistance;
		
		placePosition = wasHit && isInRange ? hitInfo.point : placePosition;
		if (isPlacing) {
			Vector3 oldUpVector = scheme.transform.up;
			Vector3 newUpVector;
			
			if (!wasHit || !isInRange) {
				Utilities.ArchRaycast(cam.position, maxDistance, cam.forward, -cam.up, out RaycastHit hit,
					10.0f);
				placePosition = hit.point;
				newUpVector = hit.normal;
			}
			else {
				newUpVector = hitInfo.normal;
			}
			scheme.transform.position = placePosition;
			

			if (Input.GetButton("Rotate Scheme")) { //Rotate scheme
				schemeYRotation += rotationSpeed * Time.deltaTime;
			}
			
			scheme.transform.up = Vector3.Lerp(oldUpVector, newUpVector, rotLerpSpeed * Time.deltaTime);
			scheme.transform.Rotate(scheme.transform.up, schemeYRotation, Space.World);

			if (Input.GetButtonDown("Fire1")) { //Accept scheme
				savedState.apply(scheme);
				
				scheme = null;
				isPlacing = false;
				schemeYRotation = 0.0f;
			}
		}
	}

	public void StartPlacing(GameObject obj) {
		bool wasHit = Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo);

		bool isInRange = Vector3.Distance(hitInfo.point, cam.position) < maxDistance;
		
		placePosition = wasHit && isInRange ? hitInfo.point : placePosition;
		
		isPlacing = true;
		scheme = Instantiate(obj, placePosition, Quaternion.identity);
		scheme.transform.up = wasHit && isInRange ? hitInfo.normal : scheme.transform.up;

		savedState = SavePrefabState(scheme);
		setSchemeState(scheme);
	}

	private ObjectTreeState SavePrefabState(GameObject obj) {
		return new ObjectTreeState(obj);
	}

	private void setSchemeState(GameObject obj) {
		ObjectState schemeState = new ObjectState {
			material = schemeMaterial,
			colliderEnable = false,
			layer = schemeLayer
		};

		ObjectTreeState.apply(obj, schemeState);
	}
}

public class ObjectTreeState { // Keeps ObjectState for every object in the subtree
	private Dictionary<Transform, ObjectState> states = new Dictionary<Transform, ObjectState>();
	
	public ObjectTreeState() {}
	
	public ObjectTreeState(GameObject obj) : this() {
		get(obj);
	}
	public void get(GameObject obj) {
		ObjectState parentState = new ObjectState(obj);
		states.Add(obj.transform, parentState);
		foreach (Transform child in obj.transform) {
			ObjectState childState = new ObjectState(child.gameObject);
			states.Add(child, childState);
		}
	}
	
	public void apply(GameObject obj) {
		states[obj.transform].apply(obj);
		foreach (Transform child in obj.transform) {
			try {
				states[child].apply(child.gameObject);
			}
			catch {}
		}
	}
	
	public static void apply(GameObject obj, ObjectState state) {
		state.apply(obj);
		foreach (Transform child in obj.transform) {
			state.apply(child.gameObject);
		}
	}
}

public class ObjectState { // Keeps object state which is changed while placing a build
	//TODO: Multiobject states
	public Material material;
	public bool colliderEnable;
	public int layer;

	public ObjectState() {}
	
	public ObjectState(GameObject obj) : this() {
		get(obj);
	}
	
	public void get(GameObject obj) {
		try {
			material = obj.GetComponent<Renderer>().material;
		} catch{}

		try {
			colliderEnable = obj.GetComponent<Collider>().enabled;
		} catch {}

		layer = obj.layer;
	}

	public void apply(GameObject obj) {
		try {
			obj.GetComponent<Renderer>().material = material;
		} catch {}

		try {
			obj.GetComponent<Collider>().enabled = colliderEnable;
		} catch {}

		obj.layer = layer;
	}
}
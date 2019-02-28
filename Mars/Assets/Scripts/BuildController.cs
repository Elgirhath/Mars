using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BuildController : MonoBehaviour {
	public GameObject prefab;
	public float maxDistance;
	public float rotLerpSpeed;
	public float rotationSpeed;
	
	public Material schemeMaterial;

	private Transform cam;
	private bool isPlacing;
	private GameObject scheme;
	private BuildingState savedState;
	
	private float schemeYRotation;
	private Vector3 placePosition;
	private int schemeLayer;

	// Update is called once per frame
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

		if (Input.GetKeyDown(KeyCode.Q) && !isPlacing) {  //TODO: Choose building via a menu or sth and not with Q
			isPlacing = true;
			scheme = Instantiate(prefab, placePosition, Quaternion.identity);
			scheme.transform.up = wasHit && isInRange ? hitInfo.normal : scheme.transform.up;

			
			savedState = SavePrefabState(scheme);
			setSchemeState(scheme);
		}

		if (isPlacing) {
			Vector3 oldUpVector = scheme.transform.up;
			Vector3 newUpVector = oldUpVector;
			
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

	private BuildingState SavePrefabState(GameObject obj) {
		return new BuildingState(obj);
	}

	private void setSchemeState(GameObject obj) {
		BuildingState schemeState = new BuildingState {
			material = schemeMaterial,
			colliderEnable = false,
			layer = schemeLayer
		};

		schemeState.apply(obj);
	}
}

public class BuildingState {
	//TODO: Multiobject states
	public Material material;
	public bool colliderEnable;
	public int layer;

	public BuildingState() {}
	
	public BuildingState(GameObject obj) : this() {
		get(obj);
	}
	
	public void get(GameObject obj) {
		material = obj.GetComponent<Renderer>().material;
		colliderEnable = obj.GetComponent<Collider>().enabled;
		layer = obj.layer;
	}

	public void apply(GameObject obj) {
		obj.GetComponent<Renderer>().material = material;
		obj.GetComponent<Collider>().enabled = colliderEnable;
		obj.layer = layer;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {
	public GameObject prefab;
	public float maxDistance;
	public float rotLerpSpeed;
	public Material schemeMaterial;

	private Material defaultMaterial;
	private Transform cam;
	private bool isPlacing;
	private GameObject scheme;
	private TerrainData terrain;

	private Vector3 placePosition;
	
	// Update is called once per frame
	void Start() {
		isPlacing = false;
		cam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform;
		terrain = Terrain.activeTerrain.terrainData;
		placePosition = Vector3.zero;
		try {
			defaultMaterial = scheme.GetComponent<Renderer>().material;
		}
		catch {}
	}
	
	void Update() {
		bool wasHit = Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo);

		bool isInRange = Vector3.Distance(hitInfo.point, cam.position) < maxDistance;

		placePosition = wasHit && isInRange ? hitInfo.point : placePosition;
		
		if (Input.GetKeyDown(KeyCode.Q) && !isPlacing) {
			isPlacing = true;
			scheme = Instantiate(prefab, placePosition, Quaternion.identity);
			scheme.transform.up = wasHit && isInRange ? hitInfo.normal : scheme.transform.up;

			try {
				scheme.GetComponent<Collider>().enabled = false;
				scheme.GetComponent<Renderer>().material = schemeMaterial;
			}
			catch { }
		}

		if (isPlacing) {
			scheme.transform.position = placePosition;
			Vector3 oldUpVector = scheme.transform.up;
			Vector3 newUpVector = wasHit && isInRange ? hitInfo.normal : oldUpVector;

			float angle = Vector3.Angle(oldUpVector, newUpVector);

			scheme.transform.up = Vector3.Lerp(oldUpVector, newUpVector, rotLerpSpeed * Time.deltaTime);

			if (Input.GetButtonDown("Fire1")) {
				try {
					scheme.GetComponent<Collider>().enabled = true;
					scheme.GetComponent<Renderer>().material = defaultMaterial;
				}
				catch {}
				
				scheme = null;
				isPlacing = false;
			}
		}
	}
}
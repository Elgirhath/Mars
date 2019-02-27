using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {
	public GameObject prefab;

	private Camera cam;
	private bool isPlacing;
	private GameObject scheme;
	private TerrainData terrain;
	
	// Update is called once per frame
	void Start() {
		isPlacing = false;
		cam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
		terrain = Terrain.activeTerrain.terrainData;
	}
	
	void Update() {
		RaycastHit hitInfo;
		Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo);

		Vector3 placePosition = hitInfo.point;
		
		if (Input.GetKeyDown(KeyCode.Q) && !isPlacing) {
			isPlacing = true;
			scheme = Instantiate(prefab, placePosition, Quaternion.Euler(0, 0, 0));
		}

		if (isPlacing) {
			scheme.transform.position = placePosition;
			scheme.transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}
}
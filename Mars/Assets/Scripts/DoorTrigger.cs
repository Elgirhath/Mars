using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {
	private Player player;
	private Collider playerCollider;
	private Vector3 oldPlayerPos;
	private BreathController o2Ctrl;
	private Mesh mesh;

	private void Start() {
		player = Player.instance;
		playerCollider = player.GetComponent<Collider>();
		oldPlayerPos = Player.instance.transform.position;
		o2Ctrl = player.GetComponent<BreathController>();
		mesh = GetComponent<MeshFilter>().mesh;
	}

	private void Update() {
		Vector3 playerPos = playerCollider.bounds.center;
		Vector3 rayDir = playerPos - oldPlayerPos;

		Ray ray = new Ray(oldPlayerPos, rayDir.normalized);

		if (Raycast(ray, rayDir.magnitude)) {
//			SwitchAir();
		}

		oldPlayerPos = playerPos;
	}

//	private void SwitchAir() {
//		o2Ctrl.air = o2Ctrl.air == outdoorAir ? capsAir : outdoorAir;
//	}

	private bool Raycast(Ray ray, float maxDistance) {
		Ray localRay = new Ray(transform.InverseTransformPoint(ray.origin), transform.InverseTransformDirection(ray.direction));
		Vector3[] vertices = mesh.vertices;
		Plane plane = new Plane(vertices[0], vertices[1], vertices[2]);
		float hitDist;
		if (plane.Raycast(localRay, out hitDist)) {
			if (Mathf.Abs(hitDist) > maxDistance)
				return false;
			
			Vector3 point = localRay.origin + localRay.direction.normalized * hitDist;
			if (Mathf.Abs(point.x) > 0.5f)
				return false;
			if (Mathf.Abs(point.y) > 0.5f)
				return false;
			return true;
		}

		return false;
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Door : MonoBehaviour {
	public enum axisEnum {
		xPositive,
		xNegative,
		zPositive,
		zNegative
	};

	public axisEnum wingAxis;
	public float maxAngle;
	public float openingTime;
	public float closeTriggerDistance;

	private Player player;
	private float timer;

	private Quaternion startRot;
	private Quaternion endRot;

	private bool rotating = false;
	private bool opened = false;

	private void Start() {
		player = Player.instance;
		
		startRot = transform.rotation;
	}

	public void Interact() {
		if (rotating || opened)
			return;
		
		Vector3 localAxis = GetLocalAxis(wingAxis);
		Vector3 globalAxis = transform.TransformDirection(localAxis);
		Vector3 playerDir = player.transform.position - transform.position;
		Vector3 playerDirProjection = Vector3.ProjectOnPlane(playerDir, transform.up);
		Vector3 rotAxisWithError = Vector3.Cross(playerDirProjection, globalAxis).normalized;
		float sign = Mathf.Sign(Vector3.Dot(rotAxisWithError, transform.up));
		Quaternion relativeEndRot = Quaternion.AngleAxis(sign * maxAngle, transform.up);
		endRot = relativeEndRot * startRot;
		
		StartRotating();
	}

	private void Update() {
		if (rotating && !opened) {
			Rotate(startRot, endRot);
		}
		else if (rotating) {
			Rotate(endRot, startRot);
		}

		if (!rotating && opened &&
		    Vector3.Distance(transform.position, player.transform.position) > closeTriggerDistance) {
			StartRotating();
		}
	}

	private Vector3 GetLocalAxis(axisEnum axis) {
		switch (axis) {
			case axisEnum.xPositive:
				return Vector3.right;
			case axisEnum.xNegative:
				return -Vector3.right;
			case axisEnum.zPositive:
				return Vector3.forward;
			case axisEnum.zNegative:
				return -Vector3.forward;
			default:
				return Vector3.zero;
		}
	}

	private void StartRotating() {
		rotating = true;
		timer = 0.0f;
	}

	private void StopRotating() {
		opened = !opened;
		rotating = false;
		timer = 0.0f;
	}

	private void Rotate(Quaternion start, Quaternion end) {
		timer += Time.deltaTime;
		transform.rotation = Quaternion.Lerp(start, end, timer / openingTime);
		if (timer >= openingTime) {
			StopRotating();
		}
	}
}

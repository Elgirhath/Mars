using System;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour {
	private string buttonName;
	private string actionName;
	private Text textField;
	private RectTransform textTransform;

	public static TooltipController instance;

	private Transform target;

	private Camera cam;

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy(gameObject);
		}

		textField = GetComponentInChildren<Text>();
		textField.gameObject.SetActive(false);
		textTransform = textField.GetComponent<RectTransform>();

		target = null;

		cam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
	}

	private void Update() {
		if (target)
			MoveToTarget();
	}

	private void MoveToTarget() {
		if (!target)
			throw new NullReferenceException();
		textTransform.anchoredPosition = cam.WorldToScreenPoint(target.position);
	}

	public void SetText(string text) {
		textField.text = text;
	}

	public void OpenPickupTooltip(Transform target, string objectName) {
		this.target = target;
		textField.text = "Press E to pick up " + objectName;
		MoveToTarget();
		
		Enable();
	}

	public void Disable() {
		textField.gameObject.SetActive(false);
		target = null;
	}

	public void Enable() {
		textField.gameObject.SetActive(true);
	}
}
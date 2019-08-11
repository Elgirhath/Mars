using System;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
	private string buttonName;
	private string actionName;
	private Text textField;
	private RectTransform textTransform;

    public static Tooltip instance { get; set; }

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

	private void Update()
    {
        if (target)
            MoveToTarget();
    }

	private void MoveToTarget() {
		if (!target)
			throw new NullReferenceException();
		textTransform.anchoredPosition = cam.WorldToScreenPoint(target.position);
	}

	public void SetText(string text)
    {
        textField.text = text;


        Debug.Log(text);
    }

    public void OpenTooltip(Transform target, string text) {
		this.target = target;
		SetText(text);
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
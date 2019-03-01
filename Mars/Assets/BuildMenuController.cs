using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuController : MonoBehaviour, Menu {
	public BuildMenuItem[] items;
	public GameObject button;

	private Transform panel;
	private GameObject gameController;
	private BuildController buildController;

	private void Start() {
		gameController = GameObject.FindGameObjectWithTag("GameController");
		buildController = gameController.GetComponent<BuildController>();
		panel = transform.GetChild(0);
		
		RemoveItems();
		AddItems();
		
		Close();
	}

	private void AddItems() {
		foreach (BuildMenuItem item in items) {
			GameObject newObj = Instantiate(button, panel);
			Button newButton = newObj.GetComponent<Button>();
			newButton.GetComponentInChildren<Text>().text = item.name;
			newButton.onClick.AddListener(delegate { StartPlacing(item); } );
		}
	}
	
	private void RemoveItems() {
		foreach (Transform item in panel) {
			Destroy(item.gameObject);
		}
	}

	private void Update() {
		if (Input.GetButtonDown("Open Building Menu"))
			Open();
	}

	private void StartPlacing(BuildMenuItem item) {
		buildController.StartPlacing(item.gameObject);

		Close();
	}

	public void Open() {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		
		foreach (Transform child in transform) { //enable all children
			child.gameObject.SetActive(true);
		}
	}

	public void Close() {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		foreach (Transform child in transform) { //disable all children
			child.gameObject.SetActive(false);
		}
	}
}

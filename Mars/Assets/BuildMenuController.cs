using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuController : MonoBehaviour, Menu {
	public BuildMenuItem[] items;
	public GameObject button;

	private GameObject gameController;
	private BuildController buildController;

	private void Start() {
		gameController = GameObject.FindGameObjectWithTag("GameController");
		Debug.Log("D: " + gameController.name);
		buildController = gameController.GetComponent<BuildController>();
		Debug.Log("C: " + buildController.gameObject.name);
		
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
		foreach (BuildMenuItem item in items) {
			GameObject newObj = Instantiate(button, transform);
			Button newButton = newObj.GetComponent<Button>();
			newButton.GetComponentInChildren<Text>().text = item.name;
			newButton.onClick.AddListener(delegate { StartPlacing(item); } );
		}
		
		Close();
	}

	private void Update() {
//		if (Input.GetButtonDown("Open Building Menu"))
//			Open();
	}

	private void StartPlacing(BuildMenuItem item) {
		buildController.StartPlacing(item.gameObject);

		Close();
	}

	public void Open() {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		
		GetComponent<Image>().enabled = true;
		foreach (Transform child in transform) {
			child.gameObject.SetActive(true);
		}
	}

	public void Close() {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		
		GetComponent<Image>().enabled = false;
		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);
		}
	}
}

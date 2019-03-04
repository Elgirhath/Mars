using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject button;

    private bool isOpened = true;

    private Transform panel;
    private GameObject gameController;
    private PlayerController playerController;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        panel = transform.GetChild(0);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		
        RemoveItems();
		
        Close();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Inventory")) {
            if (isOpened)
                Close();
            else
                Open();
        }
    }

    public void AddItem(ItemProperties item) {
        GameObject newObj = Instantiate(button, panel);
        Button newButton = newObj.GetComponent<Button>();
        newButton.GetComponentInChildren<Text>().text = item.name;
        newButton.onClick = item.onUse;
        Debug.Log(item.name);
    }
	
    private void RemoveItems() {
        foreach (Transform item in panel) {
            Destroy(item.gameObject);
        }
    }

    private void UseItem(ItemProperties item) {
        Close();
    }
    
    public void Open() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0.0f; //pause game

        isOpened = true;
		
        playerController.camLockState = true;
		
        foreach (Transform child in transform) { //enable all children
            child.gameObject.SetActive(true);
        }
    }

    public void Close() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isOpened = false;

        Time.timeScale = 1.0f; //resume game

        playerController.camLockState = false;

        foreach (Transform child in transform) { //disable all children
            child.gameObject.SetActive(false);
        }
    }
}
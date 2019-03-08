using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject button;

    private Transform panel;

    private ScrollingInfoController scrollingInfoController;

    public static Inventory instance;

    private void Awake() {
        if (!instance) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        panel = transform.GetChild(0);
        scrollingInfoController = GameObject.FindGameObjectWithTag("ScrollingInfo").GetComponent<ScrollingInfoController>();
		
        RemoveItems();
    }

    public void AddItem(Item item) {
        GameObject newObj = Instantiate(button, panel);
        Button newButton = newObj.GetComponent<Button>();
        try {
            newObj.GetComponent<Image>().sprite = item.sprite;
            newButton.GetComponentInChildren<Text>().text = "";
        }
        catch {
            newButton.GetComponentInChildren<Text>().text = item.itemName;
        }

        newButton.onClick.AddListener(item.Use);
        newButton.onClick.AddListener(delegate { RemoveButtonObject(newObj); });
        
        scrollingInfoController.AddText(item.itemName);
    }

    public void RemoveButtonObject(GameObject obj) {
        Destroy(obj);
    }
	
    private void RemoveItems() {
        foreach (Transform item in panel) {
            Destroy(item.gameObject);
        }
    }

    private void UseItem(Item item) {
        Close();
    }
    
    public void Open() {
        foreach (Transform child in transform) { //enable all children
            child.gameObject.SetActive(true);
        }
    }

    public void Close() {
        foreach (Transform child in transform) { //disable all children
            child.gameObject.SetActive(false);
        }
    }
}
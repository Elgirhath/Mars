using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanelController : MonoBehaviour
{
    private PlayerController playerController;
    private bool isOpened = false;
    private Inventory inventory;
    private ConditionPanel condition;
    
    public static PlayerPanelController instance;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start() {
        playerController = PlayerController.instance;
        inventory = Inventory.instance;
        Debug.Log(inventory.name);
        condition = ConditionPanel.instance;
        
        Close();
    }

    void Update() {
        if (Input.GetButtonDown("Inventory")) {
            if (isOpened)
                Close();
            else
                Open();
        }
    }
    
    public void Open() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0.0f; //pause game
		
        playerController.camLockState = true;

        inventory.Open();
        condition.Open();

        isOpened = true;
    }

    public void Close() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1.0f; //resume game

        playerController.camLockState = false;

        inventory.Close();
        condition.Close();

        isOpened = false;
    }
}

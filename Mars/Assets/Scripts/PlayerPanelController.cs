using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanelController : MonoBehaviour {
    /*
     * Player panel composed of Inventory and the ConditionPanel
     */
    
    private bool opened;
    
    public static PlayerPanelController instance;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start() {
        Close();
    }

    void Update() {
        if (Input.GetButtonDown("Inventory"))
        {
            if (opened)
                Close();
            else
                Open();
        }
        else if (opened && Input.GetButtonDown("Cancel"))
        {
            Close();
        }
    }
    
    public void Open() {
        /*
         * Open the panels and handle cursor, HUD and other settings
         */
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

//        Time.timeScale = 0.0f; //pause game
		
        PlayerController.instance.camLockState = true;

        Inventory.instance.Open();
        ConditionPanel.instance.Open();
        
        Crosshair.instance.active = false;
        PauseMenu.instance.block = true;

        InteractController.instance.enabled = false;

        opened = true;
        HUD.instance.active = false;
    }

    public void Close() {
        /*
         * Close the panels
         */
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

//        Time.timeScale = 1.0f; //resume game

        PlayerController.instance.camLockState = false;

        Inventory.instance.Close();
        ConditionPanel.instance.Close();
        Crosshair.instance.active = true;
        PauseMenu.instance.block = false;

        InteractController.instance.enabled = true;

        opened = false;
        HUD.instance.active = true;
    }
}

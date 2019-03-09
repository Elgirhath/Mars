using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanelController : MonoBehaviour
{
    private PlayerController playerController;
    private bool _opened;
    private Inventory inventory;
    private ConditionPanel condition;
    
    public static PlayerPanelController instance;
    private Crosshair _crosshair;
    private PauseMenu _pauseMenu;
    
    public bool opened => _opened;

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
        _crosshair = Crosshair.instance;
        _pauseMenu = PauseMenu.instance;
        
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0.0f; //pause game
		
        playerController.camLockState = true;

        inventory.Open();
        condition.Open();
        _crosshair.active = false;
        _pauseMenu.block = true;

        _opened = true;
    }

    public void Close() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1.0f; //resume game

        playerController.camLockState = false;

        inventory.Close();
        condition.Close();
        _crosshair.active = true;
        _pauseMenu.block = false;

        _opened = false;
    }
}

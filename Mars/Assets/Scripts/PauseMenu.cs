using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private Transform panel;
    private GameObject gameController;
    private PlayerController playerController;
    private Crosshair crosshair;
    private bool opened;
    private TooltipController tooltip;
    private bool _block;
    
    public static PauseMenu instance;

    public bool block
    {
        get => _block;
        set => _block = value;
    }

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        crosshair = Crosshair.instance;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        panel = transform.GetChild(0);
        panel.gameObject.SetActive(false);
        tooltip = TooltipController.instance;
        block = false;
        
        opened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!block && Input.GetButtonDown("Cancel"))
        {
            if (opened)
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
        panel.gameObject.SetActive(true);
        crosshair.active = false;
        tooltip.gameObject.SetActive(false);
        opened = true;
    }

    public void Close() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1.0f; //resume game

        playerController.camLockState = false;
        panel.gameObject.SetActive(false);
        crosshair.active = true;
        tooltip.gameObject.SetActive(true);
        opened = false;
    }
}

using Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        private Transform panel;
        private Player player;
        private Crosshair.Crosshair crosshair;
        private bool opened;
        private Tooltip.Tooltip tooltip;
    
        private bool _block;
        public bool block
        {
            get => _block;
            set => _block = value;
        }
    
        public static PauseMenu instance;

        private void Awake() {
            if (!instance)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }
    
        // Start is called before the first frame update
        void Start()
        {
            player = Player.instance;
            crosshair = Crosshair.Crosshair.instance;
            panel = transform.GetChild(0);
            panel.gameObject.SetActive(false);
            tooltip = Tooltip.Tooltip.instance;
        
            block = false;
            opened = false;

            panel.Find("Resume Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate { Close(); });
            panel.Find("Exit Game Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate { QuitGame(); });
        
        }

        // Update is called once per frame
        void Update()
        {
            if (!block && Input.GetButtonDown("Cancel")) {
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
		
            player.camLockState = true;
            panel.gameObject.SetActive(true);
            crosshair.active = false;
            tooltip.gameObject.SetActive(false);
            opened = true;
        }

        public void Close() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1.0f; //resume game

            player.camLockState = false;
            panel.gameObject.SetActive(false);
            crosshair.active = true;
            tooltip.gameObject.SetActive(true);
            opened = false;
        }
    
        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

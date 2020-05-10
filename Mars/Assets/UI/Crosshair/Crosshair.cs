using UnityEngine;

namespace UI.Crosshair
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField]
        private bool _active;

        public bool active
        {
            get => _active;
            set
            {
                _active = value;
                gameObject.SetActive(_active);
            }
        }
    
        public static Crosshair instance;
    
        private void Awake() {
            if (!instance) {
                instance = this;
            }
            else if (instance != this) {
                Destroy(gameObject);
            }
        }
    
        // Start is called before the first frame update
        void Start()
        {
            active = true;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

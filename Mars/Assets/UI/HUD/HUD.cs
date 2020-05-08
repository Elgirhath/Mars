using UnityEngine;

namespace Assets.UI.HUD
{
    public class HUD : MonoBehaviour
    {
        public static HUD instance;
        public bool active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
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
            active = true;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

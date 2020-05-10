using UnityEngine;

namespace UI.Menu
{
    public class Menu : MonoBehaviour {
	
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
}

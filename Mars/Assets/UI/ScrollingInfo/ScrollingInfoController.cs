using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.ScrollingInfo
{
    public class ScrollingInfoController : MonoBehaviour
    {
        public float lineDuration;
    
        public static ScrollingInfoController instance;
    
        private Transform panel;
    
        public GameObject textObject;
        private string defaultText;
    
        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            else if (instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            panel = transform.GetChild(0);
            foreach (Transform item in panel) {
                Destroy(item.gameObject);
            }
            defaultText = " added to inventory.";
        }

        public void AddText(string content, bool isItem)
        {
            if (isItem)
            {
                bool multipleItems = false;
                foreach (ScrollingInfoTextController line in panel.GetComponentsInChildren<ScrollingInfoTextController>())
                {
                    if (content == line.itemName)
                    {
                        // if there is a line with the same name, destroy it and add a new one with incremented quantity
                        multipleItems = true;
                        AddNewItemLine(content, line.quantity + 1);
                        Destroy(line.gameObject);
                    }
                }

                if (!multipleItems)
                {
                    // if no line with the same name has been found, add a new one with quantity = 1
                    AddNewItemLine(content, 1);
                }
            }
            else
            {
                AddNewLine(content);
            }
        }

        private void AddNewLine(string content)
        {
            GameObject newObj = Instantiate(textObject, panel);
            newObj.GetComponent<ScrollingInfoTextController>().stackTime = Time.time;
            newObj.GetComponent<Text>().text = content;
        }
    
        private void AddNewItemLine(string itemName, int quantity)
        {
            GameObject newObj = Instantiate(textObject, panel);
            ScrollingInfoTextController line = newObj.GetComponent<ScrollingInfoTextController>();
            line.SetValues(Time.time, itemName, quantity);
            if (quantity > 1)
            {
                newObj.GetComponent<Text>().text = itemName + " (x" + quantity + ")" + defaultText;
            }
            else
            {
                newObj.GetComponent<Text>().text = itemName + defaultText;
            }
        }

        // Update is called once per frame
        void Update()
        {
            bool childrenExist = false;
            Transform info;
            ScrollingInfoTextController infoController = null;
            float curTime = Time.time;
            for (var i = 0; i < panel.childCount; ++i)
            {
                info = panel.GetChild(i);

                if (!info.gameObject) continue;

                infoController = info.gameObject.GetComponent<ScrollingInfoTextController>();

                if (infoController.fading) continue;

                childrenExist = true;
                break;
            }

            if (childrenExist && curTime - infoController.stackTime > lineDuration)
            {
                infoController.Diminish();
            }
        }
    }
}

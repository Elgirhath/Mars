using UnityEngine;
using UnityEngine.UI;

public class ScrollingInfoController : MonoBehaviour
{
    public float lineDuration;
    
    public static ScrollingInfoController instance;
    
    private Transform panel;
    private GameObject gameController;
    
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
        gameController = GameObject.FindGameObjectWithTag("GameController");
        panel = transform.GetChild(0);
        foreach (Transform item in panel) {
            Destroy(item.gameObject);
        }
        defaultText = " added to inventory.";
    }

    public void AddText(string itemName)
    {
        bool multipleItems = false;
        foreach (ScrollingInfoTextController line in panel.GetComponentsInChildren<ScrollingInfoTextController>())
        {
            if (itemName == line.ItemName)
            {
                // if there is a line with the same name, destroy it and add a new one with incremented quantity
                multipleItems = true;
                AddNewLine(itemName, line.Quantity+1);
                Destroy(line.gameObject);
            }
        }

        if (!multipleItems)
        {
            // if no line with the same name has been found, add a new one with quantity = 1
            AddNewLine(itemName, 1);
        }
    }
    
    private void AddNewLine(string itemName, int quantity)
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
            if (info.gameObject)
            {
                infoController =
                    info.gameObject.GetComponent<ScrollingInfoTextController>();
                if (!infoController.Fading)
                {
                    childrenExist = true;
                    break;
                }
            }
        }

        if (childrenExist && curTime - infoController.collectTime > lineDuration)
        {
            infoController.Diminish();
        }
    }
}

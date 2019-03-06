using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingInfoController : MonoBehaviour
{
    public float lineDuration;
    private Transform oldestInfo;
    
    public static ScrollingInfoController instance;
    
    private Transform panel;
    private GameObject gameController;
    
    public GameObject textObject;
    
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
        oldestInfo = null;
    }

    public void AddText(String itemName)
    {
        GameObject newObj = Instantiate(textObject, panel);
        newObj.GetComponent<ScrollingInfoTextController>().collectTime = Time.time;
        String textString = newObj.GetComponent<Text>().text;
        textString = itemName + textString;
    }

    // Update is called once per frame
    void Update()
    {
        float curTime = Time.time;
        Debug.Log(oldestInfo);
        if (panel.childCount > 0)
        {
            oldestInfo = panel.GetChild(0);
            if (oldestInfo.gameObject && curTime - oldestInfo.gameObject.GetComponent<ScrollingInfoTextController>().collectTime > lineDuration)
            {
                Destroy(oldestInfo.gameObject);
            }
        }
    }
}

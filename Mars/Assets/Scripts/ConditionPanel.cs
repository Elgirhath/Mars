using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionPanel : MonoBehaviour {
    public static ConditionPanel instance;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

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

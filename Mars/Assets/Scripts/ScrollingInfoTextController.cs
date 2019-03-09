﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingInfoTextController : MonoBehaviour
{
    public float collectTime;
    private int quantity;
    private string itemName;
    private float fadingDuration;
    private bool fading;

    private Text textController;

    public void SetValues(float t, string name, int quan)
    {
        textController = gameObject.GetComponent<Text>();
        Fading = false;
        ItemName = name;
        collectTime = t;
        Quantity = quan;
    }

    public int Quantity
    {
        get => quantity;
        set => quantity = value;
    }

    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    public bool Fading
    {
        get => fading;
        set => fading = value;
    }

    public void Diminish()
    {
        Fading = true;
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        textController.CrossFadeAlpha(0,fadingDuration,true);
        yield return new WaitForSeconds(fadingDuration);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        fadingDuration = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

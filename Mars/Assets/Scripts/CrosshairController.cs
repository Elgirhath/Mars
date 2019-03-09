using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private bool enabled;
    
    public static CrosshairController instance;

    public bool Status
    {
        get => instance.enabled;
        set
        {
            instance.enabled = value;
            gameObject.SetActive(instance.enabled);
        }
    }
    
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
        instance.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

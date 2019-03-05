using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    private string buttonName;
    private string actionName;
    
    private static TooltipController instance;
    public static TooltipController Instance => instance;
    
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler {
    public delegate void ClickAction();

    public event ClickAction onLeftClick;
    public event ClickAction onRightClick;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            try {
                onLeftClick.Invoke();
            }
            catch {}
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            try {
                onRightClick.Invoke();
            }
            catch {}
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CapsuleAirController : MonoBehaviour {
    [SerializeField]
    public Air air;
    public Air targetAir;

    public static CapsuleAirController instance = null;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
}

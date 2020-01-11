using System;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class CapsuleAirController : MonoBehaviour {

    private AirZone airZone;
    public Air air
    {
        get
        {
            if (airZone == null)
            {
                airZone = GetComponentInChildren<AirZone>();
            }
            return airZone.air;
        }
    }

    public Air targetAir;

    public static CapsuleAirController instance = null;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void OnValidate()
    {
        air.Validate();
        targetAir.Validate();
    }
}

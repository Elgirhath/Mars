using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public static T[] GetZonesInPoint<T>()
    {
        var allZones = GameObject.FindGameObjectsWithTag("Zone");
        return allZones.Select(x => x.GetComponent<T>()).Where(x => x != null).ToArray();
    }
}
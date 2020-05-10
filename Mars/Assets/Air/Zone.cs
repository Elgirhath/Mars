using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Air
{
    public class Zone : MonoBehaviour
    {
        public static IEnumerable<T> GetZonesInPoint<T>(Vector3 point) where T : Zone
        {
            var allZones = GameObject.FindGameObjectsWithTag("Zone");
            var tZones = allZones.Select(x => x.GetComponent<T>()).Where(x => x != null);
            var tZonesInPoint = tZones.Where(
                x => x.GetComponent<Collider>().bounds.Contains(point)
            );
            return tZonesInPoint;
        }
    }
}
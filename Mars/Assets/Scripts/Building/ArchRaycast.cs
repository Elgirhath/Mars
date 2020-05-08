using UnityEngine;

namespace Assets.Scripts.Building
{
    public class Utilities {
        public static bool ArchRaycast(Vector3 origin, float radius, Vector3 startDir, Vector3 endDir, out RaycastHit hit, float angleStep) {
            Vector3 axis = Vector3.Cross(startDir, endDir);
            float angle = Vector3.Angle(startDir, endDir);
            float currentAngle = 0.0f;
            Vector3 or = startDir.normalized * radius;
            while (currentAngle < angle) {
                Vector3 end = Quaternion.AngleAxis(angleStep, axis) * or;
                Ray ray = new Ray(or + origin, end - or);
                Debug.DrawRay(origin, startDir, Color.yellow);
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
                Debug.DrawRay(origin, axis, Color.blue);
                RaycastHit rayHit;
                bool wasHit = Physics.Raycast(ray, out rayHit, (end - or).magnitude);
                if (wasHit) {
                    Debug.DrawRay(rayHit.point, Vector3.up, Color.green);
                    hit = rayHit;
                    return true;
                }

                or = end;
                currentAngle += angleStep;
            }

            hit = new RaycastHit();
            return false;
        }
    }
}
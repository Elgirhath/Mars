using Assets.UI.Menu.BuildMenu;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Building
{
    public class BuildController : MonoBehaviour {
        public float maxDistance;
        public float rotLerpSpeed;
        public float rotationSpeed;
        public Transform buildingParent = null;
	
        public Material schemeMaterial;

        private Transform cam;
        private bool isPlacing;
        private GameObject scheme;
	
        private float schemeYRotation;
        private Vector3 placePosition;
        private int schemeLayer;
        private GameObject prefab = null;
	
        void Start() {
            isPlacing = false;
            cam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform;
            placePosition = Vector3.zero;
            schemeLayer = LayerMask.NameToLayer("Ignore Raycast");
            schemeYRotation = 0.0f;
        }

        void Update() {
            bool wasHit = Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo);

            bool isInRange = Vector3.Distance(hitInfo.point, cam.position) < maxDistance;
		
            placePosition = wasHit && isInRange ? hitInfo.point : placePosition;
            if (isPlacing) {
                Vector3 oldUpVector = scheme.transform.up;
                Vector3 newUpVector;
			
                if (!wasHit || !isInRange) {
                    Utilities.ArchRaycast(cam.position, maxDistance, cam.forward, -cam.up, out RaycastHit hit,
                        10.0f);
                    placePosition = hit.point;
                    newUpVector = hit.normal;
                }
                else {
                    newUpVector = hitInfo.normal;
                }
                scheme.transform.position = placePosition;
			

                if (Input.GetButton("Rotate Scheme")) { //Rotate scheme
                    schemeYRotation += rotationSpeed * Time.deltaTime;
                }
			
                scheme.transform.up = Vector3.Lerp(oldUpVector, newUpVector, rotLerpSpeed * Time.deltaTime);
                scheme.transform.Rotate(scheme.transform.up, schemeYRotation, Space.World);

                if (Input.GetButtonDown("Fire1")) { //Accept scheme
                    Place();
                }
            }
        }

        private void Place() {
            /*
		 * Accept the position of the scheme
		 */
		
            BuildMenuItem bmi = scheme.GetComponent<BuildMenuItem>();
            bmi.AcceptScheme(prefab);
		
            prefab = null;
            scheme = null;
            isPlacing = false;
            schemeYRotation = 0.0f;
        }

        public void StartPlacing(GameObject obj) {
            /*
		 * Start placing activity, can be called within Build Menu etc.
		 */
		
            bool wasHit = Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo);

            bool isInRange = Vector3.Distance(hitInfo.point, cam.position) < maxDistance;
		
            placePosition = wasHit && isInRange ? hitInfo.point : placePosition;
		
            isPlacing = true;
            prefab = obj;
            scheme = Instantiate(obj, placePosition, Quaternion.identity, buildingParent);
            scheme.transform.up = wasHit && isInRange ? hitInfo.normal : scheme.transform.up;
            ApplySchemeState(scheme);
        }

        private void ApplySchemeState(GameObject obj) {
            /*
		 * Apply scheme material, remove unnecessary components from the object and its children
		 */
		
            RemoveComponents(obj);

            try {
                Collider collider = obj.GetComponent<Collider>();
                collider.enabled = false;
                collider.isTrigger = true;
            }
            catch{}

            try {
                obj.GetComponent<MeshRenderer>().material = schemeMaterial;
            }
            catch {}
		
            obj.layer = schemeLayer;
		
            foreach (Transform child in obj.transform) {
                try {
                    ApplySchemeState(child.gameObject);
                }
                catch {}
            }
        }

        private void RemoveComponents(GameObject obj) {
            /*
		 * Remove all scripts from the object for the time we are placing it
		 */
		
            Component[] components = obj.GetComponents<Component>();
            foreach (var component in components) {
                if (component is MeshFilter)
                    continue;
                if (component is MeshRenderer)
                    continue;
                if (component is Transform)
                    continue;
                if (component is Collider)
                    continue;
                if (component is BuildMenuItem)
                    continue;
			
                Destroy(component);
            }
        }
    }
}
using UnityEngine;

namespace Assets.Scripts.Interact
{
    public class InteractController : MonoBehaviour {
        [Tooltip("Max distance from camera in which objects can be collected")]
        public float maxCollectDistance;
        [Tooltip("Weight from 0 to 1. 0 means distance only, 1 means angle only.")]
        [Range(0.0f, 1.0f)]
        public float weight;
        [Tooltip("Max angle from camera forward in which objects can be collected")]
        [Range(0.0f, 90.0f)]
        public float maxAngle;

        private Camera cam;
        private Transform camTransform;
        private bool allowInteract;
        private Collider[] nearItems;
        private GameObject target;

			
        private Collider closestItem;
        private float closestItemFactor;

        public Material selectedMaterial;

        public static InteractController instance;

        private void Awake() {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        void Start() {
            cam = GetComponentInChildren<Camera>();
            camTransform = cam.transform;
            target = null;
		
            closestItem = null;
            closestItemFactor = 0;
        }
	
        void Update()
        {
            allowInteract = false;
            closestItem = null;
            closestItemFactor = 0;
		
            if (!HitScan())
            {
                AreaScan();
            }
		
            if (closestItem) // if any item can be collected
            {
                allowInteract = true;
                if (closestItem.gameObject != target) { // if our current target isn't the one we want to pick up - change current target
                    if(target)
                        Deselect();
                    target = closestItem.gameObject;
                    Select();
                }
            }
            else if(target)
            {
                Deselect();
                target = null;
            }
		
            if (allowInteract) {
                if (Input.GetButtonDown("Use")) {
                    target.GetComponent<Interactable>().Interact();
                }
            }
        }
	
	
        private bool HitScan()
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit)) {
                if (hit.distance <= maxCollectDistance * 0.85f)
                {
                    if (hit.transform.gameObject.GetComponent<Interactable>() != null) {
                        closestItem = hit.transform.gameObject.GetComponent<Collider>();
                        return true;
                    }
                }
            }

            return false;
        }

        private void AreaScan()
        {
            nearItems = Physics.OverlapSphere(transform.position, maxCollectDistance); //get all objects in range
            for (int i = 0; i < nearItems.Length; ++i) {
                Vector3 camItemVector = nearItems[i].transform.position - camTransform.position; //vector from camera to the object
                float angle = Vector3.Angle(camItemVector, camTransform.forward);
                if (angle > maxAngle)
                    continue;
			
                if (nearItems[i].transform.gameObject.GetComponent<Interactable>() != null) {
                    float factor = weight * camItemVector.magnitude + (1 - weight) * angle; //selecting the right object by the lowest factor
                    if (factor < closestItemFactor || !closestItem)
                    {
                        closestItemFactor = factor;
                        closestItem = nearItems[i];
                    }
                }
            }
        }
	
        void Select() {
            target.GetComponent<Interactable>().Select();
        }

        void Deselect() {
            target.GetComponent<Interactable>().Deselect();
        }
	
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ItemDropdownController : MonoBehaviour {
    private Transform handle;

    class Target {
        public Transform transform;
//        public bool hadRigidbody;
        private Renderer renderer;

        public Bounds bounds {
            get => renderer.bounds;
        }
        
//        public Rigidbody savedRigidbody;

        public Vector3 pivot {
            get => bounds.center + Vector3.down * bounds.extents.y;
        }
        public Vector3 velocity;

        public void Set(GameObject obj) {
            transform = obj.transform;
//            Rigidbody rb = obj.GetComponent<Rigidbody>();
//            hadRigidbody = rb != null;
            renderer = obj.GetComponent<Renderer>();
        }
    }

    private Target target = null;
    private Vector3 gravity;

    public static ItemDropdownController instance;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    
    /*
     If the dropped item has Rigidbody attached, the dropdown should use it and let it simulate.
     Otherwise the item should drop down until the raycast determines if the item hit something.
     */

    void Start() {
        gravity = Physics.gravity;
        handle = transform;
    }

    void Update()
    {
        if (target != null) {
            target.velocity += gravity * Time.deltaTime;
            Vector3 move = target.velocity * Time.deltaTime;
            float rayDistance = Vector3.Distance(target.bounds.center, target.pivot) + move.magnitude;
            Ray ray = new Ray(target.bounds.center, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance)) { //check if in the next iteration the item hits the ground
                float distanceToGround = Vector3.Distance(hit.point, target.pivot);
                move = Vector3.ClampMagnitude(move, distanceToGround);
                target.transform.Translate(move);
                EndDropping();
            }
            else
                target.transform.Translate(move);
        }
    }

    public void Drop(Item item) {
        GameObject obj = item.Instantiate(handle.position);

        target = new Target();
        target.Set(obj);
        target.velocity = Vector3.zero;
    }

    private void EndDropping() {
        target = null;
    }
}

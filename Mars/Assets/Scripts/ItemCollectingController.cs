using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectingController : MonoBehaviour
{
    public float maxCollectDistance;

    private Camera cam;
    private bool allowCollecting;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        allowCollecting = false;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit))
        {
            try
            {
                if (hit.transform.gameObject.GetComponent<MultiTag>().Contains("Collectible"))
                    //if(hit.transform.gameObject.CompareTag("Collectible"))    
                {
                    if (hit.distance <= maxCollectDistance)
                    {
                        target = hit.transform.gameObject;
                        allowCollecting = true;
                    }
                }
            }
            catch (UnityException)
            {
                
            }
        }

        if (allowCollecting)
        {
            if (Input.GetButtonDown("Use"))
            {
                Destroy(target);
                Debug.Log("Dodano do ekwipunku");
            }
        }
        
    }
}

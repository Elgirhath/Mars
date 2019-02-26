﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float sensitivity;
    private Camera cam;
    // Start is called before the first frame update
    void Start() {
        cam = GetComponentInChildren<Camera>();
    }
    
    void Update() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Transform tr = transform;
        transform.RotateAround(tr.position, tr.up, mouseX*sensitivity);
        cam.transform.RotateAround(cam.transform.position, -tr.right, mouseY*sensitivity);
    }
}

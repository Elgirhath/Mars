using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
	private Menu[] allMenus;

	private void Start() {
		allMenus = GetComponentsInChildren<Menu>();
	}

	private void Update() {
		if (Input.GetButtonDown("Open Building Menu")) {
			BuildMenuController m = GetComponentInChildren<BuildMenuController>();
			m.Open();
		}
	}
}
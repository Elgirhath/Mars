using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTag : MonoBehaviour {
	public string[] tags;

	public bool Contains(string tag) {
		int pos = Array.IndexOf(tags, tag);
		return pos > -1;
	}
}
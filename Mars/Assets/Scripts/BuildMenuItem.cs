using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuItem : MonoBehaviour {
	public Sprite sprite;

	public List<BuildMaterialTuple> materials;
}

[Serializable]
public class BuildMaterialTuple {
	public Item item;
	public int count;
}
using UnityEditor;
using UnityEngine;
using Water;

[CustomEditor(typeof(WaterSource))]
public class WaterSourceEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		WaterSource waterSource = (WaterSource) target;

		waterSource.falloff = EditorGUILayout.CurveField("Falloff Curve ", waterSource.falloff);
	}
}
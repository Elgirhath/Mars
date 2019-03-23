using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaterDeposit))]
public class WaterDepositEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		WaterDeposit waterDeposit = (WaterDeposit) target;

		waterDeposit.falloff = EditorGUILayout.CurveField("Falloff Curve ", waterDeposit.falloff);
	}
}
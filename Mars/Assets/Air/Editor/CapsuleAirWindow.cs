using System.Collections.Generic;
using Assets.Air;
using Assets.Air.Gases;
using Assets.Air.Pump;
using UnityEditor;
using UnityEngine;


public class CapsuleAirWindow : EditorWindow {
	[MenuItem("Window/Custom/Capsule air")]
	public static void ShowWindow() {
		GetWindow<CapsuleAirWindow>("Capsule air");
	}

	private CapsuleAirController capsule;

	private Vector2 scroll;

	private void OnGUI() {
		capsule = FindObjectOfType<CapsuleAirController>();

		scroll = GUILayout.BeginScrollView(scroll);
		
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		{
			PrintLabel("Current air");

			ShowAirGUI(capsule.air);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		{
			PrintLabel("Target air");

			ShowAirGUI(capsule.targetAir);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		
		PrintLabel("Tanks");
		ShowAllTanks();
		
		GUILayout.EndScrollView();
	}

	private void Update() {
		if (Application.isPlaying)
			Repaint();
	}

	private void PrintLabel(string text) {
		GUILayout.Space(10f);
		GUILayout.Label(text, EditorStyles.boldLabel);
		GUILayout.Space(10f);
	}


	private void ShowAirGUI(Air air) {
		Dictionary<Gas, float> gases = air.gasProportions;
		
		GUILayout.Label("Gases", EditorStyles.label);

		foreach (var gas in air.gasProportions.Keys) {
			GUILayout.BeginHorizontal();
			GUILayout.Label(gas.name, GUILayout.MinWidth(30f));
			gases[gas] = EditorGUILayout.Slider(air.gasProportions[gas], 0f, 1f);
			GUILayout.EndHorizontal();
		}

		air.gasProportions = gases;

		GUILayout.Space(20f);

		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Temperature", GUILayout.MinWidth(80f));
		air.temperature = EditorGUILayout.FloatField(air.temperature);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Pressure", GUILayout.MinWidth(80f));
		air.pressure = EditorGUILayout.FloatField(air.pressure);
		GUILayout.EndHorizontal();
	}

	private void ShowAllTanks() {
		GasTank[] tanks = capsule.GetComponentsInChildren<GasTank>();
		foreach (var tank in tanks) {
			ShowTank(tank);
		}
	}

	private void ShowTank(GasTank tank) {
		string gasName = tank.transform.parent.name;
		gasName = gasName.Split(' ')[0];
		GUILayout.BeginHorizontal();
		GUILayout.Label(gasName, GUILayout.MinWidth(40f));
		tank.gasWeight = EditorGUILayout.Slider(tank.gasWeight, 0f, tank.maxGasWeight, GUILayout.MinWidth(3f));
		GUILayout.Label("Max weight", GUILayout.MinWidth(70f));
		tank.maxGasWeight = EditorGUILayout.FloatField(tank.maxGasWeight, GUILayout.MaxWidth(40f));
		GUILayout.EndHorizontal();
	}
}
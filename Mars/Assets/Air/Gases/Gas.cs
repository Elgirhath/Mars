using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gas", menuName="Gas")]
public class Gas : ScriptableObject{
	public float heatCapacity = 1012f; // J/kg*K
	public float molarMass;
	private float gasConstant = 8.3144598f;

	public float GetMass(Air air) {
		float totalAirMass = air.GetMassFromPressure();
		float pseudoMass = air.gases[this] * molarMass;
		float airMolarMass = air.GetMolarMass();
		return pseudoMass / airMolarMass * totalAirMass;
	}
	
	public void SetMass(Air air, float mass) {
		float totalMass = air.GetMass() - GetMass(air) + mass;
		float totalMoles = GetMoles(mass);
		foreach (var gas in air.gases.Keys) {
			if (gas == this) 
				continue;
			
			totalMoles += gas.GetMoles(air);
		}
		
		float volumeRatio = GetMoles(mass) / totalMoles;
		Dictionary<Gas, float> newGases = new Dictionary<Gas, float>(air.gases);
		newGases[this] = volumeRatio;
		foreach (var pair in air.gases) {
			if (pair.Key == this) 
				continue;
			volumeRatio = pair.Key.GetMoles(air) / totalMoles;
			newGases[pair.Key] = volumeRatio;
		}

		air.gases = newGases;

		float molarMass = air.GetMolarMass();
		float density = totalMass / air.volume;
		float tempAbsolute = air.GetTemperatureAbsolute();
		float pressure = density * gasConstant * tempAbsolute / molarMass;
		air.pressure = pressure;
	}

	public float GetPartialPressure(Air air) {
		float moles = GetMoles(air);
		float totalMoles = air.GetTotalMoles();
		return moles / totalMoles * air.pressure;
	}

	public float GetMoles(Air air) {
		return GetMass(air) / molarMass;
	}
	
	public float GetMoles(float mass) {
		return mass / molarMass;
	}
	
	public float GetVolumeRatio(Air air) {
		float moles = GetMoles(air);
		float volumePct = moles / air.GetTotalMoles();
		return volumePct;
	}

	public float GetVolume(Air air) {
		float ratio = GetVolumeRatio(air);
		return ratio * air.volume;
	}
	
	public float GetVolume(Air air, float mass) {
		float moles = mass / molarMass;
		float totalMoles = air.GetTotalMoles() - GetMoles(air) + GetMoles(mass);
		float volumePct = moles / totalMoles;
		return volumePct * air.volume;
	}
}

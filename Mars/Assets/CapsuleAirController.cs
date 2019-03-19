using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleAirController : MonoBehaviour {
	public float capsuleVolume;

	public float _o2Mass;

	public float o2Mass {
		get => _o2Mass;
		set {
			_o2Mass = value;
			Recalculate();
		}
	}
	public float _co2Mass;
	public float co2Mass {
		get => _co2Mass;
		set {
			_co2Mass = value;
			Recalculate();
		}
	}
	public float _n2Mass;
	public float n2Mass {
		get => _n2Mass;
		set {
			_n2Mass = value;
			Recalculate();
		}
	}
	
	private void Recalculate() {
		pressure = GetPressure();
		o2Pct = MassToVolumeO2() / capsuleVolume;
		co2Pct = MassToVolumeCO2() / capsuleVolume;
	}
	
	[Range(0, 1)] public float o2Pct;
	[Range(0, 1)] public float co2Pct;
	
	public float _o2TargetMass;

	public float o2TargetMass {
		get => _o2TargetMass;
	}
	public float _co2TargetMass;
	public float co2TargetMass {
		get => _co2TargetMass;
	}
	public float _n2TargetMass;
	public float n2TargetMass {
		get => _n2TargetMass;
	}

	[Range(0, 1)] public float o2TargetPct;
	[Range(0, 1)] public float co2TargetPct;

	[Tooltip("In kPa")] public float pressure;
	[Tooltip("In kPa")] public float pressureTarget;

	public float temperature;
	public float temperatureTarget;
	
	private Dictionary<string, float> molarMassLUT = new Dictionary<string, float>() {
		{ "O2", 0.032f }, //kg
		{ "CO2", 0.044f },
		{ "N2", 0.028f },
		{ "Ar", 0.018f }
	};

	[HideInInspector]
	public float heatCapacity = 1012f; // J/kg*K
	private float gasConstant = 8.3144598f;

	private void Start() {
		SetInitialValues();
	}


	
	// ------------ INITIAL --------------

	private void SetInitialValues() {
		/*
		 * https://physics.stackexchange.com/a/467219/225758
		 */
		float totalMass = GetMassFromPressure();
		float o2PseudoMass = o2Pct * molarMassLUT["O2"];
		float co2PseudoMass = co2Pct * molarMassLUT["CO2"];
		float n2PseudoMass = (1- o2Pct - co2Pct) * molarMassLUT["N2"];
		float airMolarMass = GetMolarMass();
		_o2Mass = o2PseudoMass/airMolarMass * totalMass;
		_co2Mass = co2PseudoMass/airMolarMass * totalMass;
		_n2Mass = n2PseudoMass/airMolarMass * totalMass;
		
		float totalTargetMass = GetTargetMass();
		float o2TargetPseudoMass = o2TargetPct * molarMassLUT["O2"];
		float co2TargetPseudoMass = co2TargetPct * molarMassLUT["CO2"];
		float n2TargetPseudoMass = (1- o2TargetPct - co2TargetPct) * molarMassLUT["N2"];
		float airTargetMolarMass = GetTargetMolarMass();
		_o2TargetMass = o2TargetPseudoMass/airTargetMolarMass * totalTargetMass;
		_co2TargetMass = co2TargetPseudoMass/airTargetMolarMass * totalTargetMass;
		_n2TargetMass = n2TargetPseudoMass/airTargetMolarMass * totalTargetMass;
		Recalculate();
	}
	
	// ------------ CURRENT ---------------

	public float GetPressure() {
		/*
		PL: https://pl.wikipedia.org/wiki/G%C4%99sto%C5%9B%C4%87_powietrza
		Basic formula: p = ρRT/M
		ρ - density (kg/m3)
		R - universal gas constant = 8,3144
		T - temperature (K)
		M - molar mass (kg)
		
		Molar mass of our gasses:
			O2 - 32g
			CO2 - 44g
			N2 - 28g
			Ar - 18g
		*/

		float molarMass = GetMolarMass();
		float density = GetDensity();
		float tempAbsolute = GetTemperatureAbsolute();
		float pressure = density * gasConstant * tempAbsolute / molarMass;
		return pressure;
	}
	
	public float GetMolarMass() {
		float n2Pct = 1 - o2Pct - co2Pct;
	
		float molarMass = o2Pct * molarMassLUT["O2"] + co2Pct * molarMassLUT["CO2"] + n2Pct * molarMassLUT["N2"];
		return molarMass;
	}
	
	public float GetDensity() {
		float airMass = GetMass();
		return airMass / capsuleVolume;
	}

	public float GetDensityFromPressure() {
		float molarMass = GetMolarMass();
		float density = pressure * molarMass / (gasConstant * GetTemperatureAbsolute());
		return density;
	}
	
	public float GetTemperatureAbsolute() {
		float tempAbs = temperature + 273.15f;
		return tempAbs;
	}

	public float GetMass() {
		return o2Mass + co2Mass + n2Mass;
	}

	public float GetMassFromPressure() {
		float density = GetDensityFromPressure();
		return density * capsuleVolume;
	}

	private float GetTotalMoles() {
		float o2Moles = o2Mass / molarMassLUT["O2"];
		float co2Moles = co2Mass / molarMassLUT["CO2"];
		float n2Moles = n2Mass / molarMassLUT["N2"];
		return o2Moles + co2Moles + n2Moles;
	}

	public float MassToVolumeO2() {
		float moles = o2Mass / molarMassLUT["O2"];
		float volumePct = moles / GetTotalMoles();
		return volumePct * capsuleVolume;
	}
	
	public float MassToVolumeCO2() {
		float moles = co2Mass / molarMassLUT["CO2"];
		float volumePct = moles / GetTotalMoles();
		return volumePct * capsuleVolume;
	}
	
	public float MassToVolumeN2() {
		float moles = n2Mass / molarMassLUT["N2"];
		float volumePct = moles / GetTotalMoles();
		return volumePct * capsuleVolume;
	}
	
	// ------------ TARGET --------------

	public float GetTargetDensity() {
		float molarMass = GetTargetMolarMass();
		float tempAbsolute = GetTargetTemperatureAbsolute();
		float density = pressureTarget * molarMass / (gasConstant * tempAbsolute);
		return density;
	}
	
	
	public float GetTargetTemperatureAbsolute() {
		float tempAbs = temperatureTarget + 273.15f;
		return tempAbs;
	}
	
	public float GetTargetMolarMass() {
		float o2Pct = o2TargetPct;
		float co2Pct = co2TargetPct;
		float n2Pct = 1 - o2Pct - co2Pct;
	
		float molarMass = o2Pct * molarMassLUT["O2"] + co2Pct * molarMassLUT["CO2"] + n2Pct * molarMassLUT["N2"];
		return molarMass;
	}

	public float GetTargetMass() {
		return GetTargetDensity() * capsuleVolume;
	}

	public float GetTargetO2() {
		float targetMass = GetTargetMass();
		return o2TargetPct * targetMass;
	}

	public float GetTargetCO2() {
		float targetMass = GetTargetMass();
		return co2TargetPct * targetMass;
	}
	
	public float GetTargetN2() {
		float targetMass = GetTargetMass();
		return (100 - o2TargetPct - co2TargetPct) * targetMass;
	}
}

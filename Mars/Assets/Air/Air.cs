using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

[Serializable]
public class Air : Object{
	public float volume;

	[SerializeField]
	private GasDictionary _gases;

	public Dictionary<Gas, float> gases {
		get => _gases.ToDictionary();
		set => _gases = (GasDictionary) value;
	}

	[Tooltip("In kPa")] public float pressure;

	public float temperature;

	[HideInInspector]
	public float heatCapacity = 1012f; // J/kg*K
	private float gasConstant = 8.3144598f;


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
		float sum = 0f;

		foreach (var gas in gases.Keys) {
			sum += gases[gas] * gas.molarMass;
		}
		return sum;
	}
	
	public float GetDensity() {
		float airMass = GetMass();
		return airMass / volume;
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
		float sum = 0;
		foreach (var gas in gases.Keys) {
			sum += gas.GetMass(this);
		}
		return sum;
	}

	public float GetMassFromPressure() {
		float density = GetDensityFromPressure();
		return density * volume;
	}

	public float GetTotalMoles() {
		float sum = 0f;
		foreach (var gas in gases.Keys) {
			sum += gas.GetMoles(this);
		}
		return sum;
	}
	
	[Serializable]
	public class GasDictionary{
		[SerializeField]
		public GasDictionaryPosition[] array;

		public Dictionary<Gas, float> ToDictionary() {
			Dictionary<Gas, float> dictionary = new Dictionary<Gas, float>();
			foreach (var position in array) {
				dictionary.Add(position.gas, position.ratio);
			}

			return dictionary;
		}
		
		public static explicit operator GasDictionary(Dictionary<Gas, float> dictionary)
		{
			GasDictionary gasDictionary = new GasDictionary();
			gasDictionary.array = new GasDictionaryPosition[dictionary.Count];
			int i = 0;
			foreach (var pair in dictionary) {
				gasDictionary.array[i] = new GasDictionaryPosition(pair.Key, pair.Value);
				++i;
			}
			return gasDictionary;
		}
	}
	[Serializable]
	public class GasDictionaryPosition {
		[SerializeField]
		public Gas gas;
		[SerializeField]
		[Range(0f, 1f)]
		public float ratio;

		public GasDictionaryPosition(Gas gas, float ratio) {
			this.gas = gas;
			this.ratio = ratio;
		}
	}
}

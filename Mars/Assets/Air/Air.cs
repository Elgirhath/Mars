using System;
using System.Collections.Generic;
using System.Linq;
using Air.Gases;
using UnityEngine;

namespace Air
{
    [Serializable]
    public class Air
    {
        public float volume;

        [SerializeField]
        [Tooltip("Gas proportions volume-wise")]
        private SerializableGasDictionary _gasProportions;

        public Dictionary<Gas, float> gasProportions {
            get => _gasProportions.ToDictionary();
            set => _gasProportions = new SerializableGasDictionary(value);
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
	
        public float GetMolarMass()
        {
            return gasProportions.Keys.Sum(gas => gasProportions[gas] * gas.molarMass);
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

        public float GetMass()
        {
            return gasProportions.Keys.Sum(gas => gas.GetMass(this));
        }

        public float GetMassFromPressure() {
            float density = GetDensityFromPressure();
            return density * volume;
        }

        public float GetTotalMoles() {
            return gasProportions.Keys.Sum(gas => gas.GetMoles(this));
        }

        public void Validate()
        {
            if (Mathf.Abs(gasProportions.Values.Sum() - 1.0f) > 1e-6)
            {
                throw new ArgumentException("Gases percentage must sum up to 1.0");
            }
        }
	
        [Serializable]
        public class SerializableGasDictionary{
            [SerializeField]
            public GasDictionaryPosition[] elements;

            public SerializableGasDictionary(Dictionary<Gas, float> dictionary)
            {
                elements = dictionary.Select(kvp => new GasDictionaryPosition(kvp.Key, kvp.Value)).ToArray();
            }
            public Dictionary<Gas, float> ToDictionary()
            {
                return elements.ToDictionary(position => position.gas, position => position.ratio);
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
}

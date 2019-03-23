using System.Collections.Generic;
using UnityEngine;

public class PowerSupply : MonoBehaviour {
	public List<PowerGenerator> generators;
	
	[SerializeField]
	private List<PowerSocket> _receivers = null;
	public List<PowerSocket> receivers {
		get => _receivers;
	}

	public float generatedPower {
		get {
			float sum = 0f;
			foreach (var generator in generators) {
				sum += generator.GetPower();
			}

			return sum;
		}
	}

	public float consumption {
		get {
			float sum = 0f;
			foreach (var receiver in receivers) {
				sum += receiver.powerConsumption;
			}

			return sum;
		}
	}

	public bool IsPowered(PowerSocket socket) {
		float totalPowerConsumption = 0f;
		foreach (var receiver in receivers) {
			totalPowerConsumption += receiver.powerConsumption;
			if (receiver == socket) {
				return totalPowerConsumption <= generatedPower;
			}
		}

		return false;
	}

	public void AddReceiver(PowerSocket receiver) {
		/*
		 * receiver should be added as the last of its priority group
		 */
		for (int i = _receivers.Count - 1; i >= 0; i--) {
			if (_receivers[i].priority > receiver.priority)
				continue;
			_receivers.Insert(i + 1, receiver);
			return;
		}

		_receivers.Insert(0, receiver); //if the list was empty or the receiver had the lowest priority number
	}
}
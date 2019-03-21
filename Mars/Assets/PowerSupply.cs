using System.Collections.Generic;
using UnityEngine;

public class PowerSupply : MonoBehaviour {
    public List<PowerGenerator> generators;
    public List<PowerSocket> receivers;

    public float power {
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
    
    public float GetPower(PowerSocket socket) {
        float factor = Mathf.Clamp(power / consumption, 0f, 1f);
        return factor * socket.powerConsumption;
    }
}

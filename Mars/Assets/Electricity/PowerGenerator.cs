using UnityEngine;

namespace Assets.Electricity
{
    public class PowerGenerator : MonoBehaviour {
        public float maxPower;

        public float GetPower() {
            return maxPower;
        }
    }
}

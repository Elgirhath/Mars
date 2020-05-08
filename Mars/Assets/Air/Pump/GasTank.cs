using UnityEngine;

namespace Assets.Air.Pump
{
    public class GasTank : MonoBehaviour {
        public float maxGasWeight;
    
        [SerializeField]
        private float _gasWeight;
        public float gasWeight {
            get {
                return _gasWeight; 
            }
            set { _gasWeight = Mathf.Clamp(value, 0f, maxGasWeight); }
        }
    }
}

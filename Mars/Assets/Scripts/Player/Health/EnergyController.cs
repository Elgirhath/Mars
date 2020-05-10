using System.Collections;
using UnityEngine;

namespace Scripts.Player.Health
{
    public class EnergyController : MonoBehaviour
    {
        public float maxEnergy;
        public float regenerationSpeed;
        public float sprintDropSpeed;
        public float jumpDropValue;
        public float regenerationDelay;
        public float longTermEnergyLossInterval;
        public float longTermEnergyLossMultiplier;

        private Player player { get; set; }

        private Coroutine _regenerationDelayCoroutine;

        [SerializeField]
        private float _energy;
        public float energy
        {
            get => _energy;
            set
            {
                var difference = value - _energy;
                _energy = Mathf.Clamp(value, 0f, longTermEnergy);

                if (difference < 0)
                {
                    longTermEnergy += difference * longTermEnergyLossMultiplier;
                }
            }
        }

        [SerializeField]
        private float _longTermEnergy;
        public float longTermEnergy
        {
            get => _longTermEnergy;
            set
            {
                _longTermEnergy = Mathf.Clamp(value, 0f, maxEnergy);

                if (_longTermEnergy < energy)
                {
                    energy = _longTermEnergy;
                }
            }
        }

        public bool canRun => energy > 1e-4;
        public bool canJump => energy > jumpDropValue;

        private bool isRegenerating { get; set; }

        private bool _isResting;
        private bool isResting
        {
            set
            {
                if (value != _isResting && _regenerationDelayCoroutine != null)
                {
                    StopCoroutine(_regenerationDelayCoroutine);
                    _regenerationDelayCoroutine = null;
                }

                if (!_isResting && value) //starts resting
                {
                    _regenerationDelayCoroutine = StartCoroutine(StartRegeneratingDelayed());
                }

                if (!value)
                {
                    isRegenerating = false;
                }

                _isResting = value;
            }
        }

        private void Start() {
            player = GetComponent<Player>();

            InvokeRepeating(nameof(LongTermEnergyLoss),longTermEnergyLossInterval, longTermEnergyLossInterval);
        }

        private void LongTermEnergyLoss()
        {
            longTermEnergy -= 0.01f;
        }

        public void ExecuteJumpLoss()
        {
            energy -= jumpDropValue;
        }

        private void Update()
        {
            isResting = player.IsResting();

            if (player.IsRunning())
            {
                energy -= sprintDropSpeed;
            }

            if (isRegenerating)
            {
                energy += regenerationSpeed;
            }
            //Debug.Log(LockState);
        }

        private IEnumerator StartRegeneratingDelayed()
        {
            yield return new WaitForSeconds(regenerationDelay);
            isRegenerating = true;
        }
    }
}

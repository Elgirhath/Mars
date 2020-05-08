using System.Collections;
using Assets.Electricity;
using UnityEngine;

namespace Assets.Air.Pump
{
    public abstract class Pump : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField]
        private float recalculateInterval;
#pragma warning restore CS0649

        private float lastCallTimestamp;
        protected float deltaTime;

        private PowerSocket powerSocket;

        void Start()
        {
            lastCallTimestamp = Time.time;
            powerSocket = GetComponent<PowerSocket>();
            OnPumpStart();

            StartCoroutine(OnCoroutine());
        }

        protected abstract void OnPumpStart();
        protected abstract void Recalculate();

        private IEnumerator OnCoroutine()
        {
            for (; ; )
            {
                if (powerSocket.isPowered) { Recalculate(); }
                deltaTime = Time.time - lastCallTimestamp;
                lastCallTimestamp = Time.time;
                yield return new WaitForSeconds(recalculateInterval);
            }
        }
    }
}
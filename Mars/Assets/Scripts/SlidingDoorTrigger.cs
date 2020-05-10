using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class SlidingDoorTrigger : MonoBehaviour
    {
        [Serializable]
        public class TriggerEvent : UnityEvent { }

        public TriggerEvent onPlayerEnter;
        public TriggerEvent onPlayerExit;

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
                onPlayerEnter.Invoke();
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
                onPlayerExit.Invoke();
        }
    }
}
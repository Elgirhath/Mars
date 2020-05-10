using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Player.Health.Condition
{
    public class MedicalConditionManager : MonoBehaviour
    {
        [SerializeField]
        private float recalculateInterval = 1f;

        private void Start()
        {
            StartCoroutine(RecalculateConditions());
        }

        private IEnumerator RecalculateConditions()
        {
            while (true)
            {
                var conditions = ContextfullConditionFactory.Produce();

                Apply(conditions);

                yield return new WaitForSeconds(recalculateInterval);
            }
        }

        private void Apply(IEnumerable<Type> conditionTypes)
        {
            var appliedConditions = GetComponents<MedicalCondition>().Select(c => c.GetType()).ToArray();
            foreach (var conditionType in conditionTypes)
            {
                if (!appliedConditions.Contains(conditionType))
                {
                    Apply(conditionType);
                }
            }
        }

        private void Apply(Type conditionType)
        {
            gameObject.AddComponent(conditionType);
        }
    }
}

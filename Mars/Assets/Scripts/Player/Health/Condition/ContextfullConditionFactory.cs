
using System;
using System.Collections.Generic;

namespace Scripts.Player.Health.Condition
{
    public static class ContextfullConditionFactory
    {
        public static IList<Type> Produce()
        {
            var conditions = new List<Type>();
            var settings = Configuration.GetSettings();
            foreach (var setting in settings)
            {
                var isValid = setting.requirementProvider.TryGetValue(Player.instance, out float requirementValue);
                if (isValid)
                {
                    conditions.AddRange(ContextlessConditionFactory.Produce(setting.type, requirementValue));
                }
            }

            return conditions;
        }
    }
}

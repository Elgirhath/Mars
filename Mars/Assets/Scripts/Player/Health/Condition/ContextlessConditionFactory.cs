using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Player.Health.Condition
{
    public class ContextlessConditionFactory
    {
        public static IList<Type> Produce(RequirementType requirementType, float value)
        {
            var conditions = new List<Type>();
            var settings = Configuration.GetSettings().Where(s => s.type == requirementType);

            foreach (var setting in settings)
            {
                if (value >= setting.min && value <= setting.max)
                {
                    conditions.Add(setting.conditionType);
                }
            }

            return conditions;
        }
    }
}

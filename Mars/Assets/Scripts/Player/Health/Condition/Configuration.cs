using System;

namespace Scripts.Player.Health.Condition
{
    public class Configuration
    {
        private static readonly Setting[] settings = 
        {
            new Setting(RequirementType.O2VolumePercentage, new O2VolumePercentageProvider(), 0f, 10f, typeof(Breathlessness))
        };

        public static Setting[] GetSettings()
        {
            return settings;
        }

        public class Setting
        {
            public Setting(RequirementType type, IRequirementProvider requirementProvider, float min, float max, Type conditionType)
            {
                this.type = type;
                this.requirementProvider = requirementProvider;
                this.min = min;
                this.max = max;
                this.conditionType = conditionType;
            }

            public RequirementType type { get; }
            public IRequirementProvider requirementProvider { get; }
            public float min { get; }
            public float max { get; }
            public Type conditionType { get; }
        }
    }
}

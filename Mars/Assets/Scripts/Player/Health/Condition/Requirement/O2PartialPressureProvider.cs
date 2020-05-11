using System.Linq;
using Air;
using Commons;

namespace Scripts.Player.Health.Condition
{
    public class O2PartialPressureProvider : IRequirementProvider
    {
        public bool TryGetValue(Player player, out float requirementValue)
        {
            var airZone = AirZoneProvider.GetZones(player).FirstOrDefault();
            if (airZone == null)
            {
                requirementValue = 0f;
                return false;
            }

            var gasProportions = airZone.air.gasProportions;
            var o2 = gasProportions.Keys.First(g => g.name == "O2");

            requirementValue = o2.GetPartialPressure(airZone.air);
            return true;
        }
    }
}
using System.Linq;
using Air;

namespace Scripts.Player.Health.Condition
{
    public class O2VolumePercentageProvider : IRequirementProvider
    {
        public bool TryGetValue(Player player, out float requirementValue)
        {
            var airZone = Zone.GetZonesInPoint<AirZone>(player.transform.position).FirstOrDefault();

            if (airZone == null)
            {
                requirementValue = 0f;
                return false;
            }

            var gasProportions = airZone.air.gasProportions;
            var o2 = gasProportions.Keys.First(g => g.name == "O2");

            requirementValue = gasProportions[o2];
            return true;
        }
    }
}

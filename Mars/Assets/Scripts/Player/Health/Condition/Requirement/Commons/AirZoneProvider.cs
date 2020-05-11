using System.Collections.Generic;
using Air;
using Commons;

namespace Scripts.Player.Health.Condition
{
    public static class AirZoneProvider
    {
        private static readonly ZoneSearchMemory memory = new ZoneSearchMemory();

        public static ICollection<AirZone> GetZones(Player player)
        {
            var memorizedZones = memory.Get<AirZone>();
            if (memorizedZones != null) return memorizedZones;

            var zones = Zone.GetZonesInPoint<AirZone>(player.transform.position);
            memory.Save(zones);

            return zones;
        }
    }
}
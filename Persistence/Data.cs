using BepInEx.Logging;
using ProjectM;
using System;
using System.Collections.Generic;
using System.Linq;
using Wetstone.API;
using Wetstone.Hooks;

namespace VRising.PVP.Persistence
{
    public struct PVPMetrics {
        public int kills;
        public int deaths;
    }

    public static class Data
    {
        private static Dictionary<ulong, PVPMetrics> _PVPMetricsMap = new();

        public static void updatePVPMetrics(ulong steamID, int kills = 0, int deaths = 0)
        {
            if (_PVPMetricsMap.TryGetValue(steamID, out PVPMetrics PVPMetricsInstance)) {
                PVPMetricsInstance.kills += kills;
                PVPMetricsInstance.deaths += deaths;
                _PVPMetricsMap[steamID] = PVPMetricsInstance;
            } else
            {
                _PVPMetricsMap.Add(steamID, new PVPMetrics() { kills = kills, deaths = deaths});
            }
        }

        public static PVPMetrics getPVPMetrics(ulong steamID)
        {
            if (_PVPMetricsMap.TryGetValue(steamID, out PVPMetrics PVPMetricsInstance))
            {
                return PVPMetricsInstance;
            }
            else
            {
                return new PVPMetrics();
            }
        }
    }
}

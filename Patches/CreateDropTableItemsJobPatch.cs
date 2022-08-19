using BepInEx.Logging;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using VRising.PVP.Events;

namespace VRising.PVP.Patches
{
    [HarmonyPatch]
    public class CreateDropTableItemsJobPatch
    {
        private static ManualLogSource Log { get; set; }

        public static void Load(ManualLogSource log)
        {
            Log = log;
        }

        // <summary>
        // </summary>
        /*
        [HarmonyPatch(typeof(CreateDropTableItemsJob), "ExecuteDropOnGround")]
        [HarmonyPostfix]
        public static void ExecuteDropOnGround_Postfix(CreateDropTableItemsJob __instance)
        {
            Log.LogInfo("Somethng dropped on ground.");
        }
        */
    }
}

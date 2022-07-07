using BepInEx.Logging;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Wetstone.API;

namespace VRising.PVP.Patches
{
    [HarmonyPatch]
    public class DeathEventListenerSystemPatch
    {
        private static ManualLogSource Log { get; set; }

        public static void Load(ManualLogSource log)
        {
            Log = log;
        }

        [HarmonyPatch(typeof(ProjectM.DeathEventListenerSystem), "OnUpdate")]
        [HarmonyPostfix]
        public static void OnUpdate_Postfix(ProjectM.DeathEventListenerSystem __instance)
        {
            if (__instance._DeathEventQuery == null)
            {
                return;
            }

            NativeArray<DeathEvent> deathEvents = __instance._DeathEventQuery.ToComponentDataArray<DeathEvent>(Allocator.Temp);
            foreach (DeathEvent ev in deathEvents)
            {
                if (!__instance.EntityManager.HasComponent<PlayerCharacter>(ev.Died))
                {
                    continue;
                }

                PlayerCharacter player = __instance.EntityManager.GetComponentData<PlayerCharacter>(ev.Died);
                Entity userEntity = player.UserEntity._Entity;
                User user = __instance.EntityManager.GetComponentData<User>(userEntity);

                if (!user.IsConnected)
                {
                    continue;
                }

                Services.System.Respawn(ev.Died, player, userEntity);

                /*
                 * TODO: meh - sounds like the character gets spawned after we call this debug event, so the blood doesn't get updated
                 * I guess i'd need to patch whatever system handles the respawn :-(
                 */
                var character = user.LocalCharacter._Entity;
                var bloodComponent = __instance.EntityManager.GetComponentData<Blood>(character);
                Domain.Blood.BloodType bloodType = (Domain.Blood.BloodType)bloodComponent.BloodType.GuidHash;
                Domain.Blood.DebugBloodType debugBloodType = Domain.Blood.GetDebugBloodTypeByBloodType(bloodType);
                Log.LogInfo($"Setting character's blood to {debugBloodType} {bloodComponent.Quality} 100");
                Services.Buffs.SetCharacterBlood(user, debugBloodType, bloodComponent.Quality, 100);
            }
        }
    }


}

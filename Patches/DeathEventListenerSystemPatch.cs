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

        // <summary>
        // Postfix for `DeathEventListenerSystem` -> `OnUpdate` that triggers a respawn
        // for every `DeathEvent` found in the system query - if whoever `Died` (through `ev.Died`)
        // is a player and is connected.
        //
        // Also attempts to re-set the blood type, quality and value to whatever was in place
        // prior to the death, this way player don't need to call `.blood <type> <quality>` before
        // dueling again.
        // </summary>
        [HarmonyPatch(typeof(ProjectM.DeathEventListenerSystem), "OnUpdate")]
        [HarmonyPostfix]
        public static void OnUpdate_Postfix(ProjectM.DeathEventListenerSystem __instance)
        {
            var entityManager = __instance.EntityManager;

            if (__instance._DeathEventQuery == null)
            {
                return;
            }

            NativeArray<DeathEvent> deathEvents = __instance._DeathEventQuery.ToComponentDataArray<DeathEvent>(Allocator.Temp);
            foreach (DeathEvent ev in deathEvents)
            {
                if (entityManager.HasComponent<PlayerCharacter>(ev.Died))
                {
                    continue;
                }

                PlayerCharacter player = entityManager.GetComponentData<PlayerCharacter>(ev.Died);
                Entity userEntity = player.UserEntity._Entity;
                User user = entityManager.GetComponentData<User>(userEntity);

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
                var bloodComponent = entityManager.GetComponentData<Blood>(character);
                Domain.Blood.BloodType bloodType = (Domain.Blood.BloodType)bloodComponent.BloodType.GuidHash;
                Domain.Blood.DebugBloodType debugBloodType = Domain.Blood.GetDebugBloodTypeByBloodType(bloodType);
                Log.LogInfo($"Setting character's blood to {debugBloodType} {bloodComponent.Quality} 100");
                Services.Buffs.SetCharacterBlood(user, debugBloodType, bloodComponent.Quality, 100);
            }
        }
    }


}

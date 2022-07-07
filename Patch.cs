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

namespace VRising.PVP
{
    [HarmonyPatch]
    public class Patch
    {
        private static ManualLogSource Log { get; set; }

        public static void Load(ManualLogSource log)
        {
            Log = log;
        }

        private static void Respawn(Entity VictimEntity, PlayerCharacter player, Entity userEntity)
        {
            var bufferSystem = VWorld.Server.GetOrCreateSystem<EntityCommandBufferSystem>();
            var commandBufferSafe = new EntityCommandBufferSafe(Allocator.Temp)
            {
                Unsafe = bufferSystem.CreateCommandBuffer()
            };

            unsafe
            {
                var playerLocation = player.LastValidPosition;

                var bytes = stackalloc byte[Marshal.SizeOf<Domain.NullableFloat3>()];
                var bytePtr = new IntPtr(bytes);
                Marshal.StructureToPtr<Domain.NullableFloat3>(new()
                {
                    value = new float3(playerLocation.x, 0, playerLocation.y),
                    has_value = true
                }, bytePtr, false);
                var boxedBytePtr = IntPtr.Subtract(bytePtr, 0x10);

                var spawnLocation = new Il2CppSystem.Nullable<float3>(boxedBytePtr);
                var server = VWorld.Server.GetOrCreateSystem<ServerBootstrapSystem>();

                server.RespawnCharacter(commandBufferSafe, userEntity, customSpawnLocation: spawnLocation, previousCharacter: VictimEntity, fadeOutEntity: userEntity);
            }
        }


        [HarmonyPatch(typeof(DeathEventListenerSystem), "OnUpdate")]
        [HarmonyPostfix]
        public static void Postfix(DeathEventListenerSystem __instance)
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

                Respawn(ev.Died, player, userEntity);

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

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
                Log.LogWarning("No death event query?");
                return;
            }

            NativeArray<DeathEvent> deathEvents = __instance._DeathEventQuery.ToComponentDataArray<DeathEvent>(Allocator.Temp);
            foreach (DeathEvent ev in deathEvents)
            {
                if (!__instance.EntityManager.HasComponent<PlayerCharacter>(ev.Died))
                {
                    Log.LogWarning("Entity died but not player?");
                    continue;
                }

                PlayerCharacter player = __instance.EntityManager.GetComponentData<PlayerCharacter>(ev.Died);
                Entity userEntity = player.UserEntity._Entity;
                User user = __instance.EntityManager.GetComponentData<User>(userEntity);

                if (!user.IsConnected)
                {
                    Log.LogWarning("Player died but not online?");
                    continue;
                }

                Log.LogInfo("Respawn triggered.");
                Respawn(ev.Died, player, userEntity);
            }
        }
    }


}

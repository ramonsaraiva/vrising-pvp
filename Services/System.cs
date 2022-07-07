using ProjectM;
using ProjectM.Network;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Wetstone.API;

namespace VRising.PVP.Services
{
    internal class System
    {
        private static Entity EmptyEntity = new Entity();

        public static bool FindPlayer(string name, out Entity playerEntity, out Entity userEntity)
        {
            EntityManager entityManager = VWorld.Server.EntityManager;
            foreach (var UsersEntity in entityManager.CreateEntityQuery(ComponentType.ReadOnly<User>()).ToEntityArray(Allocator.Temp))
            {
                var candidateUser = entityManager.GetComponentData<User>(UsersEntity);

                string candidateName = candidateUser.CharacterName.ToString();
                if (candidateName.Equals(name))
                {
                    userEntity = UsersEntity;
                    playerEntity = candidateUser.LocalCharacter._Entity;
                    return true;
                }
            }
            playerEntity = EmptyEntity;
            userEntity = EmptyEntity;
            return false;
        }

        public static PrefabGUID GetPrefabGUID(Entity entity)
        {
            var entityManager = VWorld.Server.EntityManager;
            
            try
            {
                return entityManager.GetComponentData<PrefabGUID>(entity);
            }
            catch
            {
                return new PrefabGUID();
            }
        } 

        public static void Respawn(Entity victimEntity, PlayerCharacter player, Entity userEntity)
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

                server.RespawnCharacter(commandBufferSafe, userEntity, customSpawnLocation: spawnLocation, previousCharacter: victimEntity, fadeOutEntity: userEntity);
            }
        }
    }
}

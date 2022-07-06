using ProjectM.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Entities;
using Unity.Mathematics;
using Wetstone.API;

namespace VRising.PVP.Services
{
    internal class Movement
    {
        public static void TeleportPlayerToPosition(Entity userEntity, Entity characterEntity, float2 position)
        {

            var manager = VWorld.Server.EntityManager;
            var entity = manager.CreateEntity(
                ComponentType.ReadWrite<FromCharacter>(),
                ComponentType.ReadWrite<PlayerTeleportDebugEvent>()
            );
            manager.SetComponentData<FromCharacter>(entity, new()
            {
                User = userEntity,
                Character = characterEntity
            });
            manager.SetComponentData<PlayerTeleportDebugEvent>(entity, new()
            {
                Position = position,
                Target = PlayerTeleportDebugEvent.TeleportTarget.Self
            });
        }
    }
}

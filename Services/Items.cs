using ProjectM;
using ProjectM.Network;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Entities;
using Unity.Mathematics;
using Wetstone.API;

namespace VRising.PVP.Services
{
    internal class Items
    {
        public static Entity AddItemToInventory(Entity character, PrefabGUID guid, int amount)
        {
            unsafe
            {
                var gameData = VWorld.Server.GetExistingSystem<GameDataSystem>();
                var bytes = stackalloc byte[Marshal.SizeOf<Domain.NullableInt>()];
                var bytePtr = new IntPtr(bytes);
                Marshal.StructureToPtr<Domain.NullableInt>(new()
                {
                    value = 7,
                    has_value = true
                }, bytePtr, false);
                var boxedBytePtr = IntPtr.Subtract(bytePtr, 0x10);
                var hack = new Il2CppSystem.Nullable<int>(boxedBytePtr);
                var hasAdded = InventoryUtilitiesServer.TryAddItem(VWorld.Server.EntityManager, gameData.ItemHashLookupMap, character, guid, amount, out _, out Entity e, default, hack);
                return e;
            }
        }
    }
}

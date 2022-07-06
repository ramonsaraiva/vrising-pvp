using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Wetstone.API;
using Wetstone.Hooks;

namespace VRising.PVP
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("xyz.molenzwiebel.wetstone")]
    [Wetstone.API.Reloadable]
    public class Plugin : BasePlugin
    {
        private Harmony _hooks;

        public override void Load()
        {
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Patch.Load(Log);
            _hooks = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Wetstone.Hooks.Chat.OnChatMessage += HandleChatMessage;
        }

        public override bool Unload()
        {
            _hooks.UnpatchSelf();
            Wetstone.Hooks.Chat.OnChatMessage -= HandleChatMessage;
            return true;
        }
        public static Entity AddItemToInventory(Entity character, PrefabGUID guid, int amount)
        {
            unsafe
            {
                var gameData = VWorld.Server.GetExistingSystem<GameDataSystem>();
                var bytes = stackalloc byte[Marshal.SizeOf<Domain.FakeNull>()];
                var bytePtr = new IntPtr(bytes);
                Marshal.StructureToPtr<Domain.FakeNull>(new()
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

        public void SetCharacterBlood(User user, Domain.Blood.BloodType bloodType, float quality, int value)
        {
            // Frailed is an empty PrefabGUID, represented internally (through BloodType) by 0
            PrefabGUID source = (int)bloodType != 0 ? new PrefabGUID((int) bloodType) : new PrefabGUID();
            var bloodEvent = new ChangeBloodDebugEvent()
            {
                Amount = value,
                Quality = quality,
                Source = source
            };
            VWorld.Server.GetExistingSystem<DebugEventsSystem>().ChangeBloodEvent(user.Index, ref bloodEvent);
        }

        private void TpCommand(VChatEvent ev)
        {
            var manager = VWorld.Server.EntityManager;
            var entity = manager.CreateEntity(
                ComponentType.ReadWrite<FromCharacter>(),
                ComponentType.ReadWrite<PlayerTeleportDebugEvent>()
            );
            manager.SetComponentData<FromCharacter>(entity, new()
            {
                User = ev.SenderUserEntity,
                Character = ev.SenderCharacterEntity
            });
            manager.SetComponentData<PlayerTeleportDebugEvent>(entity, new()
            {
                Position = new float2(-960, -1225),
                Target = PlayerTeleportDebugEvent.TeleportTarget.Self
            });
            ev.User.SendSystemMessage("You were teleported.");
        }
        private void HpCommand(VChatEvent ev)
        {
            ev.SenderCharacterEntity.WithComponentData((ref Health health) =>
            {
                Log.LogInfo($"RecoveryHealth {health.MaxRecoveryHealth}");
                Log.LogInfo($"Health {health.Value}");
                health.MaxRecoveryHealth = 1;
                health.Value = health.MaxHealth;
            });
            ev.User.SendSystemMessage("You were fully healed.");
        }

        private void BloodCommand(VChatEvent ev)
        {
            SetCharacterBlood(ev.User, Domain.Blood.BloodType.Frailed, 0, 100);
            ev.User.SendSystemMessage("Blood refilled.");
            return;
        }

        private void ScholarCommand(VChatEvent ev)
        {
            SetCharacterBlood(ev.User, Domain.Blood.BloodType.Scholar, 100, 100);
            ev.User.SendSystemMessage("Blood set to scholar 100%.");
            return;
        }

        private void WarriorCommand(VChatEvent ev)
        {
            SetCharacterBlood(ev.User, Domain.Blood.BloodType.Warrior, 100, 100);
            ev.User.SendSystemMessage("Blood set to warrior 100%.");
            return;
        }

        private void WorkerCommand(VChatEvent ev)
        {
            SetCharacterBlood(ev.User, Domain.Blood.BloodType.Worker, 100, 100);
            ev.User.SendSystemMessage("Blood set to worker 100%.");
            return;
        }

        private void RogueCommand(VChatEvent ev)
        {
            SetCharacterBlood(ev.User, Domain.Blood.BloodType.Rogue, 100, 100);
            ev.User.SendSystemMessage("Blood set to rogue 100%.");
            return;
        }

        private void CreatureCommand(VChatEvent ev)
        {
            SetCharacterBlood(ev.User, Domain.Blood.BloodType.Creature, 100, 100);
            ev.User.SendSystemMessage("Blood set to creature 100%.");
            return;
        }

        private void BruteCommand(VChatEvent ev)
        {
            SetCharacterBlood(ev.User, Domain.Blood.BloodType.Brute, 100, 100);
            ev.User.SendSystemMessage("Blood set to brute 100%.");
            return;
        }

        private void WeaponsComand(VChatEvent ev)
        {
            foreach (var item in Domain.Item.DefaultItemKit)
            {
                AddItemToInventory(ev.SenderCharacterEntity, new PrefabGUID(item), 1);
            }
            ev.User.SendSystemMessage("Weapons transferred to your inventory.");
        }

        private void HandleChatMessage(VChatEvent ev)
        {
            if (!ev.Message.StartsWith(".")) return;
            if (ev.Cancelled) return;
            ev.Cancel();

            if (ev.Message.StartsWith(".hp"))
            {
                HpCommand(ev);
                return;
            }

            if (ev.Message.StartsWith(".tp"))
            {
                TpCommand(ev);
                return;
            }

            if (ev.Message.StartsWith(".weapons"))
            {
                WeaponsComand(ev);
                return;
            }

            if (ev.Message.StartsWith(".blood"))
            {
                BloodCommand(ev);
                return;
            }

            if (ev.Message.StartsWith(".scholar"))
            {
                ScholarCommand(ev);
                return;
            }

            if (ev.Message.StartsWith(".warrior"))
            {
                WarriorCommand(ev);
                return;
            }

            if (ev.Message.StartsWith(".brute"))
            {
                BruteCommand(ev);
                return;
            }

            if (ev.Message.StartsWith(".rogue"))
            {
                RogueCommand(ev);
                return;
            }

            if (ev.Message.StartsWith(".worker"))
            {
                WorkerCommand(ev);
                return;
            }

            if (ev.Message.StartsWith(".creature"))
            {
                CreatureCommand(ev);
                return;
            }
        }

    }
}

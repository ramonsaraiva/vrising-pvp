using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Commands.CommandHandler _commandHandler;

        public override void Load()
        {
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Patch.Load(Log);
            _hooks = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            // gotta somehow send the log instance to the command handler
            _commandHandler = new Commands.CommandHandler();
            Wetstone.Hooks.Chat.OnChatMessage += _commandHandler.HandleChatMessage;
        }

        public override bool Unload()
        {
            _hooks.UnpatchSelf();
            Wetstone.Hooks.Chat.OnChatMessage -= _commandHandler.HandleChatMessage;
            return true;
        }
    }
}

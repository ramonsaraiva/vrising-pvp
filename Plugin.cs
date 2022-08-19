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
using VRising.PVP.Patches;
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
        private Events.EventHandler _eventHandler;

        public override void Load()
        {
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            /*
             * TODO: this log is absurdly annoying - for some reason it is a property on the `BasePlugin`
             * class, I can't send a reference to it and I still want to use the same log instance to
             * write infos/warnings in patches and in command actions..
             */
            DeathEventListenerSystemPatch.Load(Log);
            CreateDropTableItemsJobPatch.Load(Log);
            _hooks = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            _commandHandler = new Commands.CommandHandler();
            Chat.OnChatMessage += _commandHandler.HandleChatMessage;

            _eventHandler = new Events.EventHandler(Log);
        }

        public override bool Unload()
        {
            _hooks.UnpatchSelf();
            Chat.OnChatMessage -= _commandHandler.HandleChatMessage;
            return true;
        }
    }
}

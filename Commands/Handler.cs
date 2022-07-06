using BepInEx.Logging;
using ProjectM;
using System;
using System.Collections.Generic;
using System.Linq;
using Wetstone.API;
using Wetstone.Hooks;

namespace VRising.PVP.Commands
{
    public class CommandHandler
    {
        public void HandleChatMessage(VChatEvent ev)
        {
            if (ev.Cancelled) return;
            if (!ev.Message.StartsWith(".")) return;
            ev.Cancel();

            string command = ev.Message.Split(' ')[0].Remove(0, 1).ToLower();

            string[] args = { };
            if (ev.Message.Contains(' '))
                args = ev.Message.Split(' ').Skip(1).ToArray();

            var commandMap = new Dictionary<string, Action>()
            {
                { "tp", () => Commands.Movement.TeleportCommand(ev) },
                { "weapons", () => Commands.Items.T8GearCommand(ev) },
                { "hp", () => Commands.Buffs.HpCommand(ev) },
                { "blood", () => Commands.Buffs.BloodCommand(ev, args) },
                { "cd", () => Commands.Buffs.ResetCooldownCommand(ev) },
            };
            if (commandMap.TryGetValue(command, out var action))
                action();
            else
                ev.User.SendSystemMessage($"Command `{command}` not found");
        }
    }
}

using Unity.Mathematics;
using Wetstone.API;
using Wetstone.Hooks;

namespace VRising.PVP.Commands
{
    internal class Movement
    {
        public static void TeleportCommand(VChatEvent ev)
        {
            var arenaPosition = new float2(-960, -1225);
            Services.Movement.TeleportPlayerToPosition(ev.SenderUserEntity, ev.SenderCharacterEntity, arenaPosition);
            ev.User.SendSystemMessage("You were teleported to the main arena position.");
        }
    }
}

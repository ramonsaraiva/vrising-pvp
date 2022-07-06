using ProjectM;
using Wetstone.API;
using Wetstone.Hooks;

namespace VRising.PVP.Commands
{
    internal class Items
    {
        public static void T8GearCommand(VChatEvent ev)
        {
            foreach (var item in Domain.Item.DefaultItemKit)
            {
                Services.Items.AddItemToInventory(ev.SenderCharacterEntity, new PrefabGUID(item), 1);
            }
            ev.User.SendSystemMessage("T8 gear kit transferred to your inventory.");
        }
    }
}

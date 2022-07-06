using ProjectM;
using Wetstone.API;
using Wetstone.Hooks;

namespace VRising.PVP.Commands
{
    internal class Buffs
    {
        public static void HpCommand(VChatEvent ev)
        {
            ev.SenderCharacterEntity.WithComponentData((ref Health health) =>
            {
                health.MaxRecoveryHealth = 1;
                health.Value = health.MaxHealth;
            });
            ev.User.SendSystemMessage("You were fully healed.");
        }

        public static void BloodCommand(VChatEvent ev, string[] args)
        {
            if (args.Length < 2)
            {
                ev.User.SendSystemMessage($"Usage: .blood <type> <quality>");
                return;
            }

            Domain.Blood.BloodType bloodType = Domain.Blood.GetBloodTypeByName(args[0]);
            Services.Buffs.SetCharacterBlood(ev.User, bloodType, float.Parse(args[1]), 100);
            ev.User.SendSystemMessage($"Blood set to {args[0]} {args[1]}%");
        }
    }
}

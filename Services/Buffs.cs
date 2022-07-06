using ProjectM;
using ProjectM.Network;
using Wetstone.API;

namespace VRising.PVP.Services
{
    internal class Buffs
    {
        public static void SetCharacterBlood(User user, Domain.Blood.BloodType bloodType, float quality, int value)
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
    }
}

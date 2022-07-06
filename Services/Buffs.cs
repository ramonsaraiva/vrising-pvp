using ProjectM;
using ProjectM.Network;
using Unity.Entities;
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

        public static void ResetCooldowns(Entity characterEntity)
        {
            EntityManager entityManager = VWorld.Server.EntityManager;

            var abilityBuffer = entityManager.GetBuffer<AbilityGroupSlotBuffer>(characterEntity);
            for (int abilityBufferIndex = 0; abilityBufferIndex < abilityBuffer.Length; abilityBufferIndex++)
            {
                var abilitySlot = abilityBuffer[abilityBufferIndex].GroupSlotEntity._Entity;
                var activeAbility = entityManager.GetComponentData<AbilityGroupSlot>(abilitySlot);
                var activeAbilityEntity = activeAbility.StateEntity._Entity;

                var activeAbilityPrefab = System.GetPrefabGUID(activeAbilityEntity);
                if (activeAbilityPrefab.GuidHash == 0) continue;

                var abilityStateBuffer = entityManager.GetBuffer<AbilityStateBuffer>(activeAbilityEntity);
                for (int abilityStateBufferIndex = 0; abilityStateBufferIndex < abilityStateBuffer.Length; abilityStateBufferIndex++)
                {
                    var abilityState = abilityStateBuffer[abilityStateBufferIndex].StateEntity._Entity;
                    var abilityCooldownState = entityManager.GetComponentData<AbilityCooldownState>(abilityState);
                    abilityCooldownState.CooldownEndTime = 0;
                    entityManager.SetComponentData(abilityState, abilityCooldownState);
                }
            }
        }
    }
}

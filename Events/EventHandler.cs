using BepInEx.Logging;
using ProjectM;
using Unity.Collections;
using Unity.Entities;
using Wetstone.API;

namespace VRising.PVP.Events
{
    public class EventHandler {

        private ManualLogSource Log { get; set; }

        public EventHandler(ManualLogSource log)
        {
            Log = log;
            EventDelegation.KillEventInstance += UpdatePvpMetrics;
            EventDelegation.KillEventInstance += SendKillMessage;
        }

        // <summary>
        // Updates PvP metrics for `killer` and `victim` based on a `KillEvent`
        // `killer` and `victim` are character entities
        // </summary>
        private void UpdatePvpMetrics(Entity killer, Entity victim)
        {
            var entityManager = VWorld.Server.EntityManager;

            if (!entityManager.HasComponent<PlayerCharacter>(killer) || !entityManager.HasComponent<PlayerCharacter>(victim))
                return;

            var (_, _, killerUser) = Services.System.RetrieveUserDataFromCharacter(killer);
            Persistence.Data.updatePVPMetrics(killerUser.PlatformId, kills: 1);

            var (_, _, victimUser) = Services.System.RetrieveUserDataFromCharacter(victim);
            Persistence.Data.updatePVPMetrics(victimUser.PlatformId, deaths: 1);
        }

        // <summary>
        // Sends a message informing (character name) who was the `killer` and the `victim`
        // in a `KillEvent`
        // </summary>
        private void SendKillMessage(Entity killer, Entity victim)
        {
            if (killer == victim)
                return;

            var entityManager = VWorld.Server.EntityManager;
            var (_, _, killerUser) = Services.System.RetrieveUserDataFromCharacter(killer);
            var (_, _, victimUser) = Services.System.RetrieveUserDataFromCharacter(victim);
            ServerChatUtils.SendSystemMessageToAllClients(entityManager, $"{killerUser.CharacterName} has killed {victimUser.CharacterName}");
        }
    }
}

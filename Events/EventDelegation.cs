using ProjectM;
using ProjectM.Network;
using Unity.Entities;
using Wetstone.API;

namespace VRising.PVP.Events
{
    public delegate void KillEvent(Entity killer, Entity victim);

    public class EventDelegation {
        public static KillEvent KillEventInstance;
    }
}

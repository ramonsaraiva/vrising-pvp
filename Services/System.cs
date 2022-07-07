using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using Wetstone.API;

namespace VRising.PVP.Services
{
    internal class System
    {
        private static Entity EmptyEntity = new Entity();

        public static bool FindPlayer(string name, out Entity playerEntity, out Entity userEntity)
        {
            EntityManager entityManager = VWorld.Server.EntityManager;
            foreach (var UsersEntity in entityManager.CreateEntityQuery(ComponentType.ReadOnly<User>()).ToEntityArray(Allocator.Temp))
            {
                var candidateUser = entityManager.GetComponentData<User>(UsersEntity);

                string candidateName = candidateUser.CharacterName.ToString();
                if (candidateName.Equals(name))
                {
                    userEntity = UsersEntity;
                    playerEntity = candidateUser.LocalCharacter._Entity;
                    return true;
                }
            }
            playerEntity = EmptyEntity;
            userEntity = EmptyEntity;
            return false;
        }

        public static PrefabGUID GetPrefabGUID(Entity entity)
        {
            var entityManager = VWorld.Server.EntityManager;
            
            try
            {
                return entityManager.GetComponentData<PrefabGUID>(entity);
            }
            catch
            {
                return new PrefabGUID();
            }
        } 
    }
}

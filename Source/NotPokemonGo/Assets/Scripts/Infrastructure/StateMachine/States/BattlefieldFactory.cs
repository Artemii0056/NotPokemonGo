using Characters;
using DefaultNamespace;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class BattlefieldFactory : IBattlefieldFactory
    {
        private IPlatoonFactory _platoonFactory;

        public BattlefieldFactory(IPlatoonFactory platoonFactory)
        {
            _platoonFactory = platoonFactory;
        }
        
        public Battlefield Create(SpawnPositionConfig spawnPositionConfigFirstCommand, SpawnPositionConfig spawnPositionConfigSecondCommand)
        {
            // здесь должна быть уже информация сколько юнитов в каждой группе и какие это юнити (префабы и CharacterStaticData)
            
            GameObject battlefield = new GameObject("Battlefield");
            
            GameObject platoonPosition1 = new GameObject("plattonPosition1");
            platoonPosition1.transform.position = Constants.Positions.Platoon1Position;
            GameObject platoonPosition2 = new GameObject("plattonPosition2");
            platoonPosition2.transform.position = Constants.Positions.Platoon2Position;
            platoonPosition2.transform.Rotate(Vector3.up, 180);
            
            platoonPosition1.transform.SetParent(battlefield.transform);
            platoonPosition2.transform.SetParent(battlefield.transform);
            
            Platoon platoon1 = _platoonFactory.Create(spawnPositionConfigFirstCommand, platoonPosition1.transform);
            Platoon platoon2 = _platoonFactory.Create(spawnPositionConfigSecondCommand, platoonPosition2.transform);
            
            return default;
        }
    }
}
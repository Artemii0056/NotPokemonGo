using Characters;
using Characters.Configs;
using DefaultNamespace;
using Services.StaticDataServices;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class BattlefieldFactory : IBattlefieldFactory
    {
        private IPlatoonFactory _platoonFactory;
        private IStaticDataService _staticDataService;

        public BattlefieldFactory(IPlatoonFactory platoonFactory, IStaticDataService  staticDataService)
        {
            _staticDataService = staticDataService;
            _platoonFactory = platoonFactory;
        }
        
        public Battlefield Create(SpawnPositionConfig spawnPositionConfigFirstCommand, SpawnPositionConfig spawnPositionConfigSecondCommand)
        {
            GameObject battlefieldPosition = new GameObject("Battlefield");
            
            GameObject platoonPosition1 = new GameObject("plattonPosition1");
            platoonPosition1.transform.position = Constants.Positions.Platoon1Position;
            
            GameObject platoonPosition2 = new GameObject("plattonPosition2");
            platoonPosition2.transform.position = Constants.Positions.Platoon2Position;
            
            platoonPosition1.transform.Rotate(Vector3.up, 180); 

            platoonPosition1.transform.SetParent(battlefieldPosition.transform);
            platoonPosition2.transform.SetParent(battlefieldPosition.transform);

            CharacterConfig characterConfigFirst = _staticDataService.GetCharacterConfig(CharacterType.First);
            CharacterConfig characterConfigSecond = _staticDataService.GetCharacterConfig(CharacterType.First);
            
            Platoon platoon1 = _platoonFactory.Create(spawnPositionConfigFirstCommand, platoonPosition1.transform, PlatoonType.Enemies, characterConfigFirst);
            Platoon platoon2 = _platoonFactory.Create(spawnPositionConfigSecondCommand, platoonPosition2.transform, PlatoonType.Friends, characterConfigSecond);

            Battlefield battlefield = new Battlefield(platoon1, platoon2);
            
            return battlefield;
        }
    }
}
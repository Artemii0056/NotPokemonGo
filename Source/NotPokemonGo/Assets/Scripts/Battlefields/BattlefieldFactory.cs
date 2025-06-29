using Characters;
using Characters.Configs;
using Infrastructure;
using Infrastructure.StateMachines.BattleStateMachine;
using Platoons;
using Services.BattleUnitContainers;
using Services.StaticDataServices;
using Statuses.Services;
using UnityEngine;

namespace Battlefields
{
    public class BattlefieldFactory : IBattlefieldFactory
    {
        private readonly IPlatoonFactory _platoonFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IStatusManager _statusManager;
        private readonly IBattleStateMachine _battleStateMachine;

        public BattlefieldFactory(
            IPlatoonFactory platoonFactory,
            IStaticDataService  staticDataService,
            IStatusManager statusManager,
            IBattleStateMachine battleStateMachine)
        {
            _statusManager = statusManager;
            _battleStateMachine = battleStateMachine;
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

            UnitConfig[] unitConfigFirst = new []
            {
                _staticDataService.GetUnitConfig(UnitType.Mage),
                _staticDataService.GetUnitConfig(UnitType.Mage)
            };

            UnitConfig[] unitConfigSecond = new []
            {
                _staticDataService.GetUnitConfig(UnitType.Mage),
                _staticDataService.GetUnitConfig(UnitType.Swordsman)
            };
            
            Platoon platoon1 = _platoonFactory.Create(spawnPositionConfigFirstCommand, platoonPosition1.transform, PlatoonType.Enemies, unitConfigFirst);
            Platoon platoon2 = _platoonFactory.Create(spawnPositionConfigSecondCommand, platoonPosition2.transform, PlatoonType.Friends, unitConfigSecond);

            Battlefield battlefield = new Battlefield(platoon1, platoon2, _statusManager);
            
            return battlefield;
        }
    }
}
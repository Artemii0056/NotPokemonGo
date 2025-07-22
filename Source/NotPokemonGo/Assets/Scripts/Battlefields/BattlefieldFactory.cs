using System.Collections.Generic;
using Characters;
using Characters.Configs;
using Infrastructure;
using LevelSetting;
using Platoons;
using Services.BattleUnitContainers;
using Services.StaticDataServices;
using Statuses.Services;
using UI.SpawnPositions;
using UnityEngine;

namespace Battlefields
{
    public class BattlefieldFactory : IBattlefieldFactory
    {
        private readonly IPlatoonFactory _platoonFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IStatusManager _statusManager;

        public BattlefieldFactory(
            IPlatoonFactory platoonFactory,
            IStaticDataService  staticDataService,
            IStatusManager statusManager)
        {
            _statusManager = statusManager;
            _staticDataService = staticDataService;
            _platoonFactory = platoonFactory;
        }
        
        public Battlefield Create(List<UnitType> units, LevelConfig levelConfig)
        {
            GameObject battlefieldPosition = new GameObject("Battlefield");
            
            GameObject platoonPosition1 = new GameObject("EnemiesPlatoon");
            platoonPosition1.transform.position = Constants.Positions.Platoon1Position;
            
            GameObject platoonPosition2 = new GameObject("FriendsPlatoon");
            platoonPosition2.transform.position = Constants.Positions.Platoon2Position;
            
            platoonPosition1.transform.Rotate(Vector3.up, 180); 

            platoonPosition1.transform.SetParent(battlefieldPosition.transform);
            platoonPosition2.transform.SetParent(battlefieldPosition.transform);

            UnitConfig[] unitConfigFirst = Create(units).ToArray();
            UnitConfig[] unitConfigSecond = levelConfig.LevelParts[0].Units.ToArray();
            
            PlatoonSpawnContainer friendPlatoonContainer = _staticDataService.GetSpawnPositionContainer(units.Count);
            
            PlatoonSpawnContainer enemyPlatoonContainer = _staticDataService.GetSpawnPositionContainer(levelConfig.LevelParts[0].Units.Count);
            
            Platoon platoon1 = _platoonFactory.Create(enemyPlatoonContainer, platoonPosition1.transform, PlatoonType.Enemies, unitConfigSecond);
            Platoon platoon2 = _platoonFactory.Create(friendPlatoonContainer, platoonPosition2.transform, PlatoonType.Friends, unitConfigFirst);

            Battlefield battlefield = new Battlefield(platoon1, platoon2, _statusManager);

            return battlefield;
        }

        private List<UnitConfig> Create(List<UnitType> units)
        { 
            List<UnitConfig> unitConfigs = new List<UnitConfig>();
            
            foreach (var type in units) 
                unitConfigs.Add(_staticDataService.GetUnitConfig(type));

            return unitConfigs;
        }
    }
}
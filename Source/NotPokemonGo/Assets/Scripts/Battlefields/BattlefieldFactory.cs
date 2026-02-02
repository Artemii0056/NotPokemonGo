using System.Collections.Generic;
using Characters;
using Characters.Configs;
using Factories.PlatoonFactories;
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
        
        public Battlefield Create(List<UnitType> units, LevelPartSetup levelPartSetup, out GameObject battlefieldGameObject)
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
            UnitConfig[] enemiesConfig = levelPartSetup.Units.ToArray();
            
            PlatoonSpawnContainer friendPlatoonContainer = _staticDataService.GetSpawnPositionContainer(units.Count);
            
            PlatoonSpawnContainer enemyPlatoonContainer = _staticDataService.GetSpawnPositionContainer(levelPartSetup.Units.Count);
            
            Platoon platoon1 = _platoonFactory.Create(enemyPlatoonContainer, platoonPosition1.transform, PlatoonType.Enemies, enemiesConfig);
            Platoon platoon2 = _platoonFactory.Create(friendPlatoonContainer, platoonPosition2.transform, PlatoonType.Heroes, unitConfigFirst);

            Battlefield battlefield = new Battlefield(platoon1, platoon2, _statusManager);
            
            battlefieldGameObject = battlefieldPosition;

            return battlefield;
        }

        public Battlefield Create(List<UnitType> units, Platoon friendPlatoon, LevelPartSetup levelPartSetup, out GameObject battlefieldGameObject)
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
            UnitConfig[] unitConfigSecond = levelPartSetup.Units.ToArray();
            
            PlatoonSpawnContainer enemyPlatoonContainer = _staticDataService.GetSpawnPositionContainer(levelPartSetup.Units.Count);
            PlatoonSpawnContainer friendPlatoonContainer = _staticDataService.GetSpawnPositionContainer(friendPlatoon.AliveUnits.Count);
            
            Platoon platoon1 = _platoonFactory.Create(enemyPlatoonContainer, platoonPosition1.transform, PlatoonType.Enemies, unitConfigSecond);
            Platoon platoon2 = _platoonFactory.Create(friendPlatoonContainer, platoonPosition2.transform, PlatoonType.Heroes,unitConfigFirst, friendPlatoon.AliveUnits);

            Battlefield battlefield = new Battlefield(platoon1, platoon2, _statusManager);

            battlefieldGameObject = battlefieldPosition;

            return battlefield;
        }

        public Battlefield Create(List<UnitConfig> heroConfigs, List<UnitConfig> enemiesConfigs)
        {
            GameObject battlefieldPosition = new GameObject("Battlefield");
            
            GameObject platoonPosition1 = new GameObject("EnemiesPlatoon");
            platoonPosition1.transform.position = Constants.Positions.Platoon1Position;
            
            GameObject platoonPosition2 = new GameObject("FriendsPlatoon");
            platoonPosition2.transform.position = Constants.Positions.Platoon2Position;
            
            platoonPosition1.transform.Rotate(Vector3.up, 180); 

            platoonPosition1.transform.SetParent(battlefieldPosition.transform);
            platoonPosition2.transform.SetParent(battlefieldPosition.transform);
            
            PlatoonSpawnContainer friendPlatoonContainer = _staticDataService.GetSpawnPositionContainer(heroConfigs.Count);
            
            PlatoonSpawnContainer enemyPlatoonContainer = _staticDataService.GetSpawnPositionContainer(enemiesConfigs.Count);
            
            Platoon platoon1 = _platoonFactory.Create(enemyPlatoonContainer, platoonPosition1.transform, PlatoonType.Enemies, enemiesConfigs.ToArray());
            Platoon platoon2 = _platoonFactory.Create(friendPlatoonContainer, platoonPosition2.transform, PlatoonType.Heroes, heroConfigs.ToArray());

            Battlefield battlefield = new Battlefield(platoon1, platoon2, _statusManager);
            
            //battlefieldGameObject = battlefieldPosition;

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
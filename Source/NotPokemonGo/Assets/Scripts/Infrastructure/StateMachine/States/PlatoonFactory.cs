using Characters;
using DefaultNamespace;
using Units;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class PlatoonFactory : IPlatoonFactory
    {
        public CharacterStaticData SourceConfig;
        public Unit UnitPrefab;
        private Unit sourceUnit;

        public Platoon Create(SpawnPositionConfig spawnPositionConfigFirstCommand, Transform platoonPosition)
        {
            sourceUnit = GameObject.Instantiate(UnitPrefab);
            sourceUnit.Construct(SourceConfig.Stats);
            // цикл по всем юнитам
            
            return default;
        }
    }

    public class UnitFactory
    {
        public Unit Create(Transform position, CharacterStaticData stats)
        {
            // по любому их нужно проициализировать

            return default;
        }
    }
}
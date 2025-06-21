using System;
using System.Collections.Generic;
using Characters;
using DefaultNamespace;
using Factories;
using Units;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class PlatoonFactory : IPlatoonFactory
    {
        private IUnitFactory _unitFactory;

        public PlatoonFactory(IUnitFactory unitFactory)
        {
            _unitFactory = unitFactory;
        }

        public Platoon Create(SpawnPositionConfig spawnPositionConfig, Transform platoonPosition,
            PlatoonType platoonType, CharacterConfig characterConfig)
        {
            List<Unit> units = new List<Unit>();

            switch (spawnPositionConfig.SpawnPositionType)
            {
                case SpawnPositionType.One:
                    FillUnits(units, platoonPosition, platoonType, spawnPositionConfig, characterConfig, 1);
                    break;

                case SpawnPositionType.Two:
                    FillUnits(units, platoonPosition, platoonType, spawnPositionConfig, characterConfig, 2);
                    break;

                case SpawnPositionType.Three:
                    FillUnits(units, platoonPosition, platoonType, spawnPositionConfig, characterConfig, 3);
                    break;

                case SpawnPositionType.Four:
                    FillUnits(units, platoonPosition, platoonType, spawnPositionConfig, characterConfig, 4);
                    break;

                case SpawnPositionType.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Platoon(units);
        }

        private void FillUnits(List<Unit> units, Transform platoonPosition, PlatoonType platoonType,
            SpawnPositionConfig spawnPositionConfig, CharacterConfig characterConfig, int unitCount)
        {
            Transform[] unitPosition = spawnPositionConfig.Prefab.GetComponentsInChildren<Transform>();

            for (int i = 0; i < unitCount; i++)
            {
                if (i < unitPosition.Length)
                {
                    units.Add(_unitFactory.Create(unitPosition[i], platoonPosition, characterConfig, platoonType));
                }
            }
        }
    }
}
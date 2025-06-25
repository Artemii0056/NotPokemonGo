using System;
using System.Collections.Generic;
using Abilities;
using Characters;
using Factories;
using Units;
using UnityEngine;

namespace Platoons
{
    public class PlatoonFactory : IPlatoonFactory
    {
        private IUnitFactory _unitFactory;
        private readonly IAbilityApplicatorService _abilityApplicatorService;
        private IAbilityProvider _abilityProvider;
        private ISourceProvider _sourceProvider;

        public PlatoonFactory(IUnitFactory unitFactory, IAbilityApplicatorService abilityApplicatorService, IAbilityProvider abilityProvider, ISourceProvider sourceProvider)
        {
            _unitFactory = unitFactory;
            _abilityApplicatorService = abilityApplicatorService;
            _abilityProvider = abilityProvider;
            _sourceProvider = sourceProvider;
        }

        public Platoon Create(SpawnPositionConfig spawnPositionConfig, Transform platoonPosition,
            PlatoonType platoonType, UnitConfig unitConfig)
        {
            List<Unit> units = new List<Unit>();

            switch (spawnPositionConfig.SpawnPositionType)
            {
                case SpawnPositionType.One:
                    FillUnits(units, platoonPosition, platoonType, spawnPositionConfig, unitConfig, 1);
                    break;

                case SpawnPositionType.Two:
                    FillUnits(units, platoonPosition, platoonType, spawnPositionConfig, unitConfig, 2);
                    break;

                case SpawnPositionType.Three:
                    FillUnits(units, platoonPosition, platoonType, spawnPositionConfig, unitConfig, 3);
                    break;

                case SpawnPositionType.Four:
                    FillUnits(units, platoonPosition, platoonType, spawnPositionConfig, unitConfig, 4);
                    break;

                case SpawnPositionType.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Platoon(units, platoonType, _abilityApplicatorService, _sourceProvider, _abilityProvider);
        }

        private void FillUnits(List<Unit> units, Transform platoonPosition, PlatoonType platoonType,
            SpawnPositionConfig spawnPositionConfig, UnitConfig unitConfig, int unitCount)
        {
            SpawnPoint[] unitPosition = spawnPositionConfig.PositionContainer.GetComponentsInChildren<SpawnPoint>();
            
            for (int i = 0; i < unitCount; i++)
            {
                if (i < unitPosition.Length)
                {
                    units.Add(_unitFactory.Create(unitPosition[i].transform.position, platoonPosition, unitConfig, platoonType));
                }
            }
        }
    }
}
using System.Collections.Generic;
using Characters;
using Factories;
using UI.SpawnPositions;
using Units;
using UnityEngine;

namespace Platoons
{
    public class PlatoonFactory : IPlatoonFactory
    {
        private IUnitFactory _unitFactory;

        public PlatoonFactory(IUnitFactory unitFactory) =>
            _unitFactory = unitFactory;

        public Platoon Create(
            PlatoonSpawnContainer container,
            Transform platoonPosition,
            PlatoonType platoonType,
            UnitConfig[] unitConfig)
        {
            List<Unit> units;

            units = FillUnits(platoonPosition, platoonType, container, unitConfig);

            return new Platoon(units, platoonType);
        }
        
        public Platoon Create(
            PlatoonSpawnContainer container,
            Transform platoonPosition,
            PlatoonType platoonType,
            UnitConfig[] unitConfig,
            List<Unit> unitsToRecreate)
        {
            List<Unit> units  =  FillUnits(platoonPosition, platoonType, container, unitConfig, unitsToRecreate);

            return new Platoon(units, platoonType);
        }
        
        private List<Unit> FillUnits(
            Transform platoonPosition,
            PlatoonType platoonType,
            PlatoonSpawnContainer container,
            UnitConfig[] unitConfig,
            List<Unit> unitsToRecreate)
        {
            List<Unit> units = new List<Unit>();

            SpawnPoint[] unitPosition = container.SpawnPoints.ToArray();

            for (int i = 0; i < unitPosition.Length; i++)
            {
                if (i < unitPosition.Length)
                    units.Add(_unitFactory.Create(unitPosition[i].transform.position, platoonPosition, unitConfig[i], platoonType, unitsToRecreate[i].Stats)); //TODO Или все же создать новых юнитов с хп/мп от старых.
            }

            return units;
        }

        private List<Unit> FillUnits(
            Transform platoonPosition,
            PlatoonType platoonType,
            PlatoonSpawnContainer container,
            UnitConfig[] unitConfig)
        {
            List<Unit> units = new List<Unit>();

            SpawnPoint[] unitPosition = container.SpawnPoints.ToArray();

            for (int i = 0; i < unitPosition.Length; i++)
            {
                if (i < unitPosition.Length)
                    units.Add(_unitFactory.Create(unitPosition[i].transform.position, platoonPosition, unitConfig[i], platoonType)); //TODO Или все же создать новых юнитов с хп/мп от старых.
            }

            return units;
        }
    }
}
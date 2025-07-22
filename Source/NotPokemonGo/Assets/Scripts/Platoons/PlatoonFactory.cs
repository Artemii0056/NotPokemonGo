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
            List<Unit> units = new List<Unit>();
            
            FillUnits(units, platoonPosition, platoonType, container, unitConfig);

            return new Platoon(units, platoonType);
        }
        
        private void FillUnits(
            List<Unit> units,
            Transform platoonPosition,
            PlatoonType platoonType,
            PlatoonSpawnContainer container,
            UnitConfig[] unitConfig)
        {
            SpawnPoint[] unitPosition = container.SpawnPoints.ToArray();
            
            for (int i = 0; i < unitPosition.Length; i++)
            {
                if (i < unitPosition.Length) 
                    units.Add(_unitFactory.Create(unitPosition[i].transform.position, platoonPosition, unitConfig[i], platoonType));
            }
        }
    }
}
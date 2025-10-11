using System;
using System.Collections.Generic;
using Platoons;
using Unit = Units.Unit;

namespace Services.BattleSessionService
{
    public class UnitReadyService : IUnitReadyService
    {
        private List<Unit> _units;
        private Platoon _friendsPlatoon;
        private Platoon _enemiesPlatoon;

        public UnitReadyService() => 
            _units = new List<Unit>();

        public bool HasUnits => _units.Count > 0;

        public void SetPlatoons(Platoon friends, Platoon enemies)
        {
            _friendsPlatoon = friends; // удалять мертвых
            _enemiesPlatoon = enemies;
            
            foreach (var unit in friends.AliveUnits) 
                unit.Prepared += OnUnitPrepared;
            
            foreach (var unit in enemies.AliveUnits) 
                unit.Prepared += OnUnitPrepared;
        }

        public Unit GiveReadyUnit()
        {
            if (HasUnits == false)
                throw new Exception("No unit selected");

            var unit = _units[0];
            _units.Remove(unit); 
            return unit;
        }

        public void Discard() 
        {
            if (_friendsPlatoon == null) //TODO Перенести логику
                return;
            
            foreach (var unit in _friendsPlatoon.AliveUnits) 
                unit.Prepared -= OnUnitPrepared;
            
            foreach (var unit in _enemiesPlatoon.AliveUnits) 
                unit.Prepared -= OnUnitPrepared;
            
            _units.Clear();
        }

        private void OnUnitPrepared(Unit unit)
        {
            _units.Add(unit);
        }
    }
}
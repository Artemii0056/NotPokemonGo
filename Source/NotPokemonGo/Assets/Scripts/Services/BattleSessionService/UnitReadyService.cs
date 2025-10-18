using System;
using System.Collections.Generic;
using Unit = Units.Unit;

namespace Services.BattleSessionService
{
    public class UnitReadyService : IUnitReadyService
    {
        private List<Unit> _units;
        private List<Unit>  _friendsUnits;
        private List<Unit>  _enemiesUnits;

        public UnitReadyService() =>
            _units = new List<Unit>();

        public bool HasUnits => _units.Count > 0;

        public void SetPlatoons(List<Unit> friends, List<Unit>  enemies)
        {
            _friendsUnits = friends;
            _enemiesUnits = enemies;

            foreach (var unit in friends)
            {
                unit.Prepared += OnUnitPrepared;
                unit.Death += OnUnitDead;
            }

            foreach (var unit in enemies)
            {
                unit.Prepared += OnUnitPrepared;
                unit.Death += OnUnitDead;
            }
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
            if (_friendsUnits == null) //TODO Перенести логику
                return;

            foreach (var unit in _friendsUnits)
            {
                unit.Prepared -= OnUnitPrepared;
                unit.Death -= OnUnitDead;
            }

            foreach (var unit in _enemiesUnits)
            {
                unit.Prepared -= OnUnitPrepared;
                unit.Death -= OnUnitDead;
            }

            _units.Clear();
        }

        private void OnUnitPrepared(Unit unit) =>
            _units.Add(unit);

        private void OnUnitDead(Unit unit)
        {
            unit.Death -= OnUnitDead;
            _units.Remove(unit);
        }
    }
}
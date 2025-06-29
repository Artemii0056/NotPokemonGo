using System.Collections.Generic;
using Battlefields;
using Units;

namespace Services.BattleUnitContainers
{
    public class BattleUnitContainer : IBattleUnitContainer
    {
        private List<Unit> _units = new List<Unit>();

        public void Add(Unit unit)
        {
            if (_units.Contains(unit) == false)
                _units.Add(unit);
        }

        public Unit Give()
        {
            if (_units.Count > 0)
            {
                Unit unit = _units[0];
                _units.Remove(unit);
                return unit;
            }

            return null;
        }

        public void Reset()
        {
            _units.Clear();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Units;

namespace Platoons
{
    public class Platoon
    {
        private readonly List<Unit> _units;

        public event Action<Unit> UnitPrepared;

        public Platoon(List<Unit> units, PlatoonType platoonType)
        {
            _units = units;
            PlatoonType = platoonType;
        }

        public PlatoonType PlatoonType { get; private set; }

        public List<Unit> Units => _units.ToList();

        public void Enable()
        {
            foreach (Unit unit in _units)
            {
                unit.Prepared += OnUnitPrepared;
            }
        }

        public void Disable()
        {
            foreach (Unit unit in _units)
                unit.Prepared -= OnUnitPrepared;
        }

        public void Tick()
        {
            foreach (Unit unit in _units)
                unit.Tick();
        }

        private void OnUnitPrepared(Unit unit) =>
            UnitPrepared?.Invoke(unit);
    }
}
using System.Collections.Generic;
using System.Linq;
using Units;

namespace Platoons
{
    public class Platoon
    {
        private List<Unit> _units;

        public Platoon(List<Unit> units, PlatoonType type)
        {
            _units = units;
            Type = type;
        }

        public PlatoonType Type { get; private set; }

        public List<Unit> AliveUnits => _units.Where(unit => unit.IsAlive).ToList();

        public bool HaveUnits => _units.Any(x => x.IsAlive);

        public void Tick()
        {
            foreach (Unit unit in AliveUnits)
                unit.Tick();
        }
    }
}
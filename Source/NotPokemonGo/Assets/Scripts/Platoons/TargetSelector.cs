using System;
using System.Collections.Generic;
using Abilities;
using Units;

namespace Platoons
{
    public class TargetSelector : ITargetSelector
    {
        private Platoon _first;
        private Platoon _second;
        
        public void SetPlatoons(Platoon platoon, Platoon platoon2)
        {
            _first = platoon;
            _second = platoon2;
        }

        public IReadOnlyList<Unit> GetTargets(TargetMode targetMode, Unit source, Unit primaryTarget)
        {
            if (_first == null || _second == null)
                throw new InvalidOperationException("Platoons are not set.");

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var result = new List<Unit>();

            var sourcePlatoon = GetPlatoonByType(source.PlatoonType);
            var enemyPlatoon = GetEnemyPlatoon(source.PlatoonType);

            switch (targetMode)
            {
                case TargetMode.Self:
                    result.Add(source);
                    break;

                case TargetMode.PrimaryTarget:
                    if (primaryTarget == null)
                        throw new InvalidOperationException("Primary target is null.");

                    result.Add(primaryTarget);
                    break;

                case TargetMode.AllEnemies:
                    result.AddRange(enemyPlatoon.AliveUnits);
                    break;

                case TargetMode.AllAllies:
                    result.AddRange(sourcePlatoon.AliveUnits);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(targetMode), targetMode, null);
            }

            return result;
        }

        public Unit GetRandomEnemyTarget()
        {
            var aliveUnits = GetEnemyPlatoon(PlatoonType.Heroes).AliveUnits; //TODO По какой то причине союзники  во вражеском платуне 
            
            return aliveUnits[UnityEngine.Random.Range(0, aliveUnits.Count)];
        }

        private Platoon GetPlatoonByType(PlatoonType platoonType)
        {
            if (_first.Type == platoonType)
                return _first;

            if (_second.Type == platoonType)
                return _second;

            throw new InvalidOperationException($"Platoon with type '{platoonType}' was not found.");
        }
        
        private Platoon GetEnemyPlatoon(PlatoonType sourceType)
        {
            if (_first.Type == sourceType)
                return _second;

            if (_second.Type == sourceType)
                return _first;

            throw new InvalidOperationException($"Enemy platoon for source type '{sourceType}' was not found.");
        }
    }
}
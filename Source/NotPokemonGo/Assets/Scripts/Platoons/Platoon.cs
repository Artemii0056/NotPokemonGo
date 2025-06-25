using System.Collections.Generic;
using System.Linq;
using Abilities;
using Abilities.MV;
using Units;
using UnityEngine;

namespace Platoons
{
    public class Platoon
    {
        private readonly List<Unit> _units;
        private readonly PlatoonType _platoonType;
        private readonly IAbilityApplicatorService _abilityApplicatorService;

        public Platoon(List<Unit> units, PlatoonType platoonType, IAbilityApplicatorService abilityApplicatorService)
        {
            _units = units;
            _platoonType = platoonType;
            _abilityApplicatorService = abilityApplicatorService;
        }

        public List<Unit> Units => _units.ToList();
        public bool IsAlive { get; private set; }

        public void Attack(List<Unit> targets)
        {
            if (_platoonType == PlatoonType.Enemies)
            {
                foreach (Unit unit in _units)
                {
                    if (unit.Step.IsReadyToAct)
                    {
                        foreach (AbilityModel abilityModel in unit.AbilityModels)
                        {
                            if (abilityModel.IsReady)
                            {
                                _abilityApplicatorService.RememberSource(unit);
                                _abilityApplicatorService.Remember(abilityModel);
                                _abilityApplicatorService.Apply(targets.ToArray());
                            }
                        }

                        unit.Step.ResetCurrentValue();
                    }
                }
            }
        }

        public void Tick(float deltaTime)
        {
            foreach (Unit unit in _units)
            {
                unit.Tick(deltaTime);
            }
        }
    }
}
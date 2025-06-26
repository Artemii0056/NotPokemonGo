using System.Collections.Generic;
using System.Linq;
using Abilities;
using Abilities.MV;
using Unity.VisualScripting;
using UnityEngine;
using Unit = Units.Unit;

namespace Platoons
{
    public class Platoon
    {
        private readonly List<Unit> _units;
        private readonly PlatoonType _platoonType;
        private readonly IAbilityApplicatorService _abilityApplicatorService;
        private ISourceProvider _sourceProvider;
        private IAbilityProvider _abilityProvider;

        public Platoon(List<Unit> units, PlatoonType platoonType, IAbilityApplicatorService abilityApplicatorService, ISourceProvider sourceProvider, IAbilityProvider abilityProvider)
        {
            _units = units;
            _platoonType = platoonType;
            _abilityApplicatorService = abilityApplicatorService;
            _sourceProvider = sourceProvider;
            _abilityProvider = abilityProvider;
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
                                _sourceProvider.Remember(unit);
                                _abilityProvider.Remember(abilityModel);
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
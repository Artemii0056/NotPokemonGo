using Abilities;
using Abilities.MV;
using UnityEngine;

namespace Services.AbilityServices
{
    public class AbilityPhaseService
    {
        private AbilityModel _currentAbility;
        private int _currentPhaseIndex;
        
        private IAbilityApplicatorService _abilityApplicatorService;
        private ITargetSelector _targetSelector;

        public AbilityPhaseService(IAbilityApplicatorService abilityApplicatorService, ITargetSelector targetSelector)
        {
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;
        }

        public void Initialize(AbilityModel abilityModel)
        {
            _currentAbility = abilityModel;
            _currentPhaseIndex = 0;
        }

        public void OnNextTrigger()
        {
            if (_currentAbility == null || _currentPhaseIndex >= _currentAbility.Phases.Count)
                return;

            AbilityPhase phase = _currentAbility.Phases[_currentPhaseIndex];
            
            if (phase.CastamentSetup.HasSetupData)
                _abilityApplicatorService.Apply(phase.CastamentSetup, _targetSelector.GetTargets(_currentAbility.TargetMode).ToArray()); // Тут таргет мод должен быть у фазы 

            if (phase.ArmamentSetup.HasSetupData)
                _abilityApplicatorService.Apply(phase.ArmamentSetup, _targetSelector.GetTargets(_currentAbility.TargetMode).ToArray());

            _currentPhaseIndex++;
        }

        public void Reset()
        {
            _currentAbility = null;
            _currentPhaseIndex = 0;
        }
    }
}
using System.Diagnostics;
using Abilities;
using Abilities.MV;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Services.AbilityServices
{
    public class AbilityPhaseService
    {
        private IAbilityApplicatorService _abilityApplicatorService;
        private ITargetSelector _targetSelector;

        public AbilityPhaseService(IAbilityApplicatorService abilityApplicatorService, ITargetSelector targetSelector)
        {
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;
        }

        public void OnNext(AbilityPhase phase)
        {
            if (phase.CastamentSetup.HasSetupData)
                _abilityApplicatorService.Apply(phase.CastamentSetup, _targetSelector.GetTargets(phase.TargetMode).ToArray()); 
            
            if (phase.ArmamentSetup.HasSetupData)
                _abilityApplicatorService.Apply(phase.ArmamentSetup, _targetSelector.GetTargets(phase.TargetMode).ToArray());
        }
    }
}
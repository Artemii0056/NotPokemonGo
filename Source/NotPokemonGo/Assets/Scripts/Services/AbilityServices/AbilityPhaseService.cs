using System.Diagnostics;
using Abilities;
using Abilities.MV;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Services.AbilityServices
{
    public class AbilityPhaseService //TODO Эта залупа не нужна больше
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
            Debug.Log(phase == null);
            
            if (phase.CastamentSetup.HasSetupData)
                _abilityApplicatorService.Apply(phase.CastamentSetup, _targetSelector.GetTargets(phase.TargetMode).ToArray()); 
            
            if (phase.ArmamentSetup.HasSetupData)
                _abilityApplicatorService.Apply(phase.ArmamentSetup, _targetSelector.GetTargets(phase.TargetMode).ToArray());
        }
    }
}
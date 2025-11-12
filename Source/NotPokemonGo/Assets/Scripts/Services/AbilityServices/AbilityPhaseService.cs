using Abilities;

namespace Services.AbilityServices
{
    public class AbilityPhaseService
    {
        private readonly IAbilityApplicatorService _abilityApplicatorService;
        private readonly ITargetSelector _targetSelector;

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
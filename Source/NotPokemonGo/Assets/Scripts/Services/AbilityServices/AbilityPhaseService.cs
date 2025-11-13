using Abilities;
using Abilities.AbilityActions.Castaments;
using Infrastructure.ReactionSystem;
using ReactionSystems;
using Units;

namespace Services.AbilityServices
{
    public class AbilityPhaseService
    {
        private readonly IAbilityApplicatorService _abilityApplicatorService;
        private readonly ITargetSelector _targetSelector;
        private readonly IReactionService _reactionService;

        public AbilityPhaseService(
            IAbilityApplicatorService abilityApplicatorService,
            ITargetSelector targetSelector,
            IReactionService reactionService)
        {
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;
            _reactionService = reactionService;
        }

        public void OnNext(AbilityPhase phase, Unit source, Unit target)
        {
            //Unit target = _targetSelector.Target; //TODO Тут как будто не обойтись без селектора

            var setup = phase.CastamentSetup;

            if (setup.HasSetupData == false)
                return;

            var context = new ReactionContext(source, target, setup.EffectsSetup[0], phase);

            if (_reactionService.TryReact(context))
                return;

            _abilityApplicatorService.Apply(setup, source,
                _targetSelector.GetTargets(phase.TargetMode, target).ToArray());

            if (phase.ArmamentSetup.HasSetupData)
                _abilityApplicatorService.Apply(phase.ArmamentSetup, source,
                    _targetSelector.GetTargets(phase.TargetMode, target).ToArray());
        }

        private void HandleCastamentSetup(CastamentSetup setup)
        {
        }
    }
}
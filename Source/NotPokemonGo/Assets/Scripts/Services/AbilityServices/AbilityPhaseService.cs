using Abilities.Configs;
using Armaments;
using Castaments;
using ReactionSystems;
using Units;
using UnityEngine;

namespace Services.AbilityServices
{
    public class AbilityPhaseService
    {
        private readonly ICastamentApplicator _castamentApplicator;
        private readonly IArmamentApplicator _armamentApplicator;
        private readonly ITargetSelector _targetSelector;
        private readonly IReactionService _reactionService;

        public AbilityPhaseService(
            ICastamentApplicator castamentApplicator,
            IArmamentApplicator armamentApplicator,
            ITargetSelector targetSelector,
            IReactionService reactionService)
        {
            _castamentApplicator = castamentApplicator;
            _armamentApplicator = armamentApplicator;
            _targetSelector = targetSelector;
            _reactionService = reactionService;
        }

        public void OnNext(AbilityPhase phase, Unit source, Unit target)
        {
            var setup = phase.CastamentSetup;

            if (setup.HasSetupData)
            {
                var context = new ReactionContext(source, target, setup.EffectsSetup[0], phase);
                _reactionService.TryReact(context);
                return;
            }

            if (phase.ArmamentSetup.HasSetupData)
                _armamentApplicator.Apply(phase.ArmamentSetup, phase.ArmamentSetup.FlyingType, source, 
                    _targetSelector.GetTargets(phase.TargetMode, target).ToArray());

            if (phase.CastamentSetup.HasSetupData)
                _castamentApplicator.Apply(phase.CastamentSetup, source, 
                    _targetSelector.GetTargets(phase.TargetMode, target).ToArray());
        }
    }
}
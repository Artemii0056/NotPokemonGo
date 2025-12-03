using Abilities;
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
        private readonly ICastamentApplicatorService _castamentApplicatorService;
        private readonly IArmamentApplicatorService _armamentApplicatorService;
        private readonly ITargetSelector _targetSelector;
        private readonly IReactionService _reactionService;

        public AbilityPhaseService(
            ICastamentApplicatorService castamentApplicatorService,
            IArmamentApplicatorService armamentApplicatorService,
            ITargetSelector targetSelector,
            IReactionService reactionService)
        {
            _castamentApplicatorService = castamentApplicatorService;
            _armamentApplicatorService = armamentApplicatorService;
            _targetSelector = targetSelector;
            _reactionService = reactionService;
        }

        public void OnNext(AbilityPhase phase, Unit source, Unit target)
        {
            //Unit target = _targetSelector.Target; //TODO Тут как будто не обойтись без селектора

            var setup = phase.CastamentSetup;

            // if (setup.HasSetupData == false)
            //     return;
            //
            // var context = new ReactionContext(source, target, setup.EffectsSetup[0], phase);
            //
            // if (_reactionService.TryReact(context))
            //     return;

            _castamentApplicatorService.Apply(setup, source,
                _targetSelector.GetTargets(phase.TargetMode, target).ToArray());

            if (phase.CastamentSetup.HasSetupData)
                _castamentApplicatorService.Apply(phase.CastamentSetup, source,
                    _targetSelector.GetTargets(phase.TargetMode, target).ToArray());

            if (phase.ArmamentSetup.HasSetupData)
            {
                _armamentApplicatorService.Apply(phase.ArmamentSetup, source,
                    _targetSelector.GetTargets(phase.TargetMode, target).ToArray());
            }
        }

        private void HandleCastamentSetup(CastamentSetup setup)
        {
        }
    }
}
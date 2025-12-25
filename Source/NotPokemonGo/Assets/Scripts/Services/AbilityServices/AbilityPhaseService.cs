using System;
using Abilities.Configs;
using Castaments;
using ReactionSystems;
using Units;

namespace Services.AbilityServices
{
    public class AbilityPhaseService
    {
        private readonly ICastamentApplicator _castamentApplicator;
        private readonly ITargetSelector _targetSelector;
        private readonly IReactionService _reactionService;
        
        public event Action<AbilityPhase> ArmamentRequested;
        public event Action<AbilityPhase> CastamentRequested;

        public AbilityPhaseService(
            ICastamentApplicator castamentApplicator,
            ITargetSelector targetSelector,
            IReactionService reactionService)
        {
            _castamentApplicator = castamentApplicator;
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
                ArmamentRequested?.Invoke(phase); //TODO 
            
            if (phase.CastamentSetup.HasSetupData) 
                CastamentRequested?.Invoke(phase); //TODO

            // if (phase.ArmamentSetup.HasSetupData)
            //     _armamentApplicator.Apply(phase.ArmamentSetup, phase.ArmamentSetup.FlyingType, source, 
            //         _targetSelector.GetTargets(phase.TargetMode, target).ToArray());
            
            if (phase.CastamentSetup.HasSetupData)
                _castamentApplicator.Apply(phase.CastamentSetup, source, 
                    _targetSelector.GetTargets(phase.TargetMode, target).ToArray());
        }
    }
}
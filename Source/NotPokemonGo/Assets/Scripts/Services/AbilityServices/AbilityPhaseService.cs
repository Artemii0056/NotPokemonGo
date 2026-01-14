using System;
using System.Linq;
using Abilities.Configs;
using Abilities.Signals;
using Armaments;
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
        
        public ArmamentSetup LastRequestedArmamentSetup { get; private set; }

        public AbilityPhaseService(
            ICastamentApplicator castamentApplicator,
            ITargetSelector targetSelector,
            IReactionService reactionService)
        {
            _castamentApplicator = castamentApplicator;
            _targetSelector = targetSelector;
            _reactionService = reactionService;
        }
        
        public void OnSignal(AbilityPhase phase, PhaseSignal signal, Unit source, Unit target)
        {
            if (phase == null) 
                return;
            
            if (signal == PhaseSignal.None) 
                return;

            var action = phase.SignalActions?.Find(a => a.Signal == signal);
            
            if (action == null) 
                return;

            if (action.HasCastament)
            {
                var firstEffect = action.CastamentSetup.EffectsSetup.FirstOrDefault();
                
                if (firstEffect != null)
                {
                    var reactionContext = new ReactionContext(source, target, firstEffect, phase);
                    if (_reactionService.TryReact(reactionContext))
                        return;
                }
            }

            if (action.HasArmament)
            {
                LastRequestedArmamentSetup = action.ArmamentSetup;
                ArmamentRequested?.Invoke(phase);
                LastRequestedArmamentSetup = null;
            }

            if (action.HasCastament)
            {
                var targets = _targetSelector.GetTargets(action.TargetMode, target).ToArray();
                _castamentApplicator.Apply(action.CastamentSetup, source, targets);
            }
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
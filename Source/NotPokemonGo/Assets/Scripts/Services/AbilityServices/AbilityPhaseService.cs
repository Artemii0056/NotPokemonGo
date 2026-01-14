using System;
using System.Linq;
using Abilities.Configs;
using Abilities.Signals;
using Castaments;
using ReactionSystems;
using Units;
using UnityEngine;

namespace Services.AbilityServices
{
    public sealed class AbilityPhaseService
    {
        private readonly ICastamentApplicator _castamentApplicator;
        private readonly ITargetSelector _targetSelector;
        private readonly IReactionService _reactionService;

        public event Action<ArmamentRequest> ArmamentRequested;

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
            Debug.Log($"[PhaseService] OnSignal {signal}, phase={(phase?.AnimationClip ? phase.AnimationClip.name : "NULL")}");
            
            if (phase == null) return;
            if (signal == PhaseSignal.None) return;
            if (source == null || target == null) return;

            var actions = phase.SignalActions?
                .Where(a => a != null && a.Signal == signal)
                .ToArray();

            if (actions == null || actions.Length == 0)
                return;

            foreach (var action in actions)
            {
                if (action.HasCastament)
                {
                    var firstEffect = action.CastamentSetup.EffectsSetup.FirstOrDefault();
                    if (firstEffect != null)
                    {
                        var reactionContext = new ReactionContext(source, target, firstEffect, phase);
                        if (_reactionService.TryReact(reactionContext))
                            continue;
                    }

                    var targets = _targetSelector.GetTargets(action.TargetMode, target).ToArray();
                    _castamentApplicator.Apply(action.CastamentSetup, source, targets);
                }

                if (action.HasArmament)
                {
                    var targets = _targetSelector.GetTargets(action.TargetMode, target).ToArray();
                    
                    Debug.Log($"[PhaseService] ArmamentRequested signal={signal},  mode={action.TargetMode}");
                    ArmamentRequested?.Invoke(new ArmamentRequest(action.ArmamentSetup, source, targets, phase));
                }
            }
        }

        public void OnNext(AbilityPhase phase, Unit source, Unit target)
        {
            OnSignal(phase, PhaseSignal.Attack1, source, target);
        }
    }
}

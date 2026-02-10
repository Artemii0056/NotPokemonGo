using System.Collections.Generic;
using Abilities.Configs;
using Abilities.Signals;
using Armaments.Movers;
using Services.AbilityServices;
using Spawners.Spawner;
using UnityEngine;

namespace Abilities.Runtime.Policies
{
    public class VolleyComposerPolicy : AbilityPolicyBase 
    {
        private readonly IArmamentSpawner _armamentSpawner;

        private AbilityContext _context;

        private AbilityPhase _activePhase;

        private bool _isReady;
        private bool _finishSeenForActivePhase; //нахрена оно тока

        public VolleyComposerPolicy(IArmamentSpawner armamentSpawner)
        {
            _armamentSpawner = armamentSpawner;
            _isReady = false;
        }

        public override void OnPhaseStart(AbilityContext ctx, AbilityPhase phase) => 
            _activePhase = phase;

        public override void OnAbilityStart(AbilityContext context)
        {
            Debug.Log("Volley Composer Started");
            
            _context = context;

            AbilityPhaseService phaseService = context?.AnimatorTrigger?.PhaseService;

            if (phaseService != null)
                phaseService.ArmamentRequested += OnArmamentRequested;
        }

        private void OnArmamentRequested(ArmamentRequest request)
        {
            Debug.Log("OnArmamentRequested");
            
            List<Transform> spawnPositions = request.Source.AbilitiesPositions;

            for (int i = 0; i < 3; i++)
            {
                foreach (var ctx in ArmamentRequestMapper.EnumerateContexts(request, spawnPositions[i]))
                {
                    IArmamentMover mover = _armamentSpawner.Create(ctx);

                    _context.Movers.Add(mover);
                }
            }

            _isReady = true;
        }

        public override void OnSignal(AbilityContext ctx, PhaseSignal signal) 
        {
            if (signal == PhaseSignal.Finish && ctx != null && ctx.CurrentPhase == _activePhase)
                _finishSeenForActivePhase = true;
        }

        public override void OnAbilityStop(AbilityContext ctx)
        {
            var phaseService = ctx?.AnimatorTrigger?.PhaseService;

            if (phaseService != null)
                phaseService.ArmamentRequested -= OnArmamentRequested;

            _context = null;
            _activePhase = null;
            _finishSeenForActivePhase = false;
            
            Debug.Log("_currentCount");
        }

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
        {
            Debug.Log("CanFinishPhase");
            
            if (phase == null)
                return true;
            
            if (!_finishSeenForActivePhase)
                return false;

            return _isReady;
        }
    }
}
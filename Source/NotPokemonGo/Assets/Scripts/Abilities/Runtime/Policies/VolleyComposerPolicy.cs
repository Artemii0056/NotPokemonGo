using System.Collections.Generic;
using Abilities.Configs;
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

        private bool _isReady;

        public VolleyComposerPolicy(IArmamentSpawner armamentSpawner)
        {
            _armamentSpawner = armamentSpawner;
            _isReady = false;
        }

        public override bool CanUseAbility(AbilityContext ctx) => 
            ctx.CurrentPhase.SignalActions[0].HasArmament;

        public override void OnAbilityStart(AbilityContext context)
        {
            if (context.Movers.Count > 0)
            {
                _isReady = true;
                return ;
            }
            
            _context = context;

            AbilityPhaseService phaseService = context?.AnimatorTrigger?.PhaseService;

            if (phaseService != null)
                phaseService.ArmamentRequested += OnArmamentRequested;
        }

        private void OnArmamentRequested(ArmamentRequest request)
        {
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

        public override void OnAbilityStop(AbilityContext ctx)
        {
            var phaseService = ctx?.AnimatorTrigger?.PhaseService;

            if (phaseService != null)
                phaseService.ArmamentRequested -= OnArmamentRequested;

            _context = null;
        }

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
        {
            if (phase == null)
                return true;
            
            return _isReady;
        }
    }
}
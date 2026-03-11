using System.Collections.Generic;
using System.Threading;
using Armaments.Movers;
using Cysharp.Threading.Tasks;
using Services.AbilityServices;
using Spawners.Spawner;
using UnityEngine;

namespace Abilities.Flow.Steps
{
    public sealed class ComposeVolleyStep : IAbilityStep
    {
        private readonly IArmamentSpawner _spawner;
        private readonly int _maxShots;

        public ComposeVolleyStep(IArmamentSpawner spawner, int maxShots = int.MaxValue)
        {
            _spawner = spawner;
            _maxShots = maxShots;
        }

        public UniTask Execute(AbilityExecutionContext ctx, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            AbilityPhaseService phaseService = ctx.Source.AnimatorTrigger.PhaseService;

            void OnArmamentRequested(ArmamentRequest request)
            {
                if (ctx.PreparedMovers.Count >= _maxShots)
                    return;

                List<Transform> spawnPositions = request.Source.AbilitiesPositions;
                if (spawnPositions == null || spawnPositions.Count == 0)
                    spawnPositions = new List<Transform> { request.Source.abilityPos };

                for (int i = 0; i < spawnPositions.Count && ctx.PreparedMovers.Count < _maxShots; i++)
                {
                    foreach (var aCtx in ArmamentRequestMapper.EnumerateContexts(request, spawnPositions[i]))
                    {
                        if (ctx.PreparedMovers.Count >= _maxShots)
                            break;

                        IArmamentMover mover = _spawner.Create(aCtx);

                        if (mover is IAbilityScopeOwnedMover owned)
                            owned.SetScopeId(ctx.Scope?.Id ?? 0);

                        ctx.PreparedMovers.Add(mover);
                    }
                }
            }

            phaseService.ArmamentRequested += OnArmamentRequested;
            ctx.AddCleanup(() => phaseService.ArmamentRequested -= OnArmamentRequested);

            return UniTask.CompletedTask;
        }
    }
}
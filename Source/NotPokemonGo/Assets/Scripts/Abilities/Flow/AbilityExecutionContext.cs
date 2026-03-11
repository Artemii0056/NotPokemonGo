using System;
using System.Collections.Generic;
using Abilities.Core;
using Abilities.Runtime.Impact;
using Armaments.Movers;
using Effects;
using QteSystem;
using QteSystem.TestQte;
using ReactionSystems;
using Services.AbilityServices;
using Spawners.Spawner;
using Units;
using UnityEngine;

namespace Abilities.Flow
{
    public sealed class AbilityExecutionContext : IDisposable
    {
        public AbilityRunScope Scope { get; set; }

        private readonly List<Action> _cleanup = new();
        private bool _disposed;

        public AbilityExecutionContext(
            Unit source,
            Unit target,
            AbilityPhaseService phaseService,
            IQteService qteService,
            IArmamentSpawner armamentSpawner,
            IReactionService reactionService,
            IEffectsApplier effectsApplier)
        {
            Source = source;
            Target = target;
            PhaseService = phaseService;
            QteService = qteService;
            ArmamentSpawner = armamentSpawner;
            ReactionService = reactionService;

            ImpactResolver = new BaseArmamentImpactResolver(effectsApplier);
        }

        public Unit Source { get; }
        public Unit Target { get; }

        public AbilityPhaseService PhaseService { get; }
        public IQteService QteService { get; }
        public IArmamentSpawner ArmamentSpawner { get; }
        public IReactionService ReactionService { get; }
        public BaseArmamentImpactResolver ImpactResolver { get; }

        public List<IArmamentMover> PreparedMovers { get; } = new();
        public List<IArmamentMover> ActiveMovers { get; } = new();

        public int TotalShots { get; set; }
        public int BlockedShots { get; set; }

        public QteResult QteResult { get; set; } = QteResult.Default;

        public void AddCleanup(Action action)
        {
            if (action == null || _disposed) return;
            _cleanup.Add(action);
        }

        public void AddCleanup(IDisposable disposable)
        {
            if (disposable == null || _disposed)
            {
                disposable?.Dispose();
                return;
            }

            if (Scope != null)
            {
                Scope.Add(disposable);
                return;
            }

            _cleanup.Add(disposable.Dispose);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try { Scope?.Dispose(); } catch { }
            Scope = null;

            for (int i = _cleanup.Count - 1; i >= 0; i--)
            {
                try { _cleanup[i]?.Invoke(); }
                catch (Exception e) { Debug.LogException(e); }
            }

            _cleanup.Clear();
        }
    }
}

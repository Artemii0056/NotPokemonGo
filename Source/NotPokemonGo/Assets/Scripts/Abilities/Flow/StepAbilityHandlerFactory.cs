using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Flow.Steps;
using Abilities.MV;
using Effects;
using QteSystem;
using ReactionSystems;
using Spawners.Spawner;
using Units;
using UnityEngine;

namespace Abilities.Flow
{
    /// <summary>
    /// Единственная фабрика хендлеров. Legacy полностью удалён.
    /// Сборка идёт только через step pipeline.
    /// </summary>
    public sealed class StepAbilityHandlerFactory : IAbilityHandlerFactory
    {
        private readonly IQteService _qte;
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly IReactionService _reactions;
        private readonly IEffectsApplier _effects;

        public StepAbilityHandlerFactory(
            IQteService qte,
            IArmamentSpawner armamentSpawner,
            IReactionService reactions,
            IEffectsApplier effects)
        {
            _qte = qte;
            _armamentSpawner = armamentSpawner;
            _reactions = reactions;
            _effects = effects;
        }

        public IAbilityHandler Create(Unit source, Unit target, AbilityModel model)
        {
            if (model == null)
                return null;

            if (source == null)
            {
                Debug.LogError("[StepFactory] Source is null");
                return null;
            }

            if (source.AnimatorTrigger == null)
            {
                Debug.LogError("[StepFactory] Source.AnimatorTrigger is null");
                return null;
            }

            if (source.AnimatorTrigger.PhaseService == null)
            {
                Debug.LogError("[StepFactory] Source.AnimatorTrigger.PhaseService is null");
                return null;
            }

            var ctx = new AbilityExecutionContext(
                source,
                target,
                source.AnimatorTrigger.PhaseService,
                _qte,
                _armamentSpawner,
                _reactions,
                _effects);

            List<IAbilityStep> steps = BuildSteps(model);
            if (steps.Count == 0)
            {
                Debug.LogError($"[StepFactory] No steps were built for ability: {model.AbilityType}");
                return null;
            }

            var executor = new AbilityPipelineExecutor(steps.ToArray());
            var handler = new PipelineAbilityHandler(executor, ctx);
            handler.BindAbility(model);

            Debug.Log($"[AbilityRouter] {model.AbilityType}: STEP");
            return handler;
        }

        private static List<IAbilityStep> BuildSteps(AbilityModel model)
        {
            var steps = new List<IAbilityStep>(capacity: 16);

            if (model.Parts == null || model.Parts.Count == 0)
                return steps;

            for (int p = 0; p < model.Parts.Count; p++)
            {
                var part = model.Parts[p];
                if (part == null || part.AbilityPhases == null)
                    continue;

                for (int i = 0; i < part.AbilityPhases.Count; i++)
                {
                    var phase = part.AbilityPhases[i];
                    if (phase == null)
                        continue;

                    steps.Add(new PlayPhaseStep(phase));
                }
            }

            return steps;
        }
    }
}

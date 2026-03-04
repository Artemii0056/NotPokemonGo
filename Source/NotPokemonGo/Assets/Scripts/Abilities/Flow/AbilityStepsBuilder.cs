using System;
using System.Collections.Generic;
using Abilities.Flow.Steps;
using Abilities.StepConfigs;

namespace Abilities.Flow
{
    public static class AbilityStepsBuilder
    {
        public static bool TryBuildSteps(
            AbilityDefinitionConfig definition,
            out List<IAbilityStep> steps,
            out string error)
        {
            if (definition == null)
            {
                steps = null;
                error = "Definition is null";
                return false;
            }

            if (definition.Steps == null || definition.Steps.Count == 0)
            {
                steps = null;
                error = $"Definition {definition.name} has no steps";
                return false;
            }

            steps = new List<IAbilityStep>(definition.Steps.Count);

            for (int i = 0; i < definition.Steps.Count; i++)
            {
                AbilityStepConfig cfg = definition.Steps[i];
                if (cfg == null)
                    continue;

                switch (cfg)
                {
                    case PlayPhaseStepConfig playPhase:
                        if (playPhase.Phase == null)
                        {
                            error = $"PlayPhaseStepConfig at index {i} has null Phase";
                            return false;
                        }
                        steps.Add(new PlayPhaseStep(playPhase.Phase));
                        break;

                    case RememberStartPositionStepConfig:
                        steps.Add(new RememberStartPositionStep());
                        break;

                    default:
                        error = $"Unsupported step config type: {cfg.GetType().Name}";
                        return false;
                }
            }

            if (steps.Count == 0)
            {
                error = $"Definition {definition.name} produced 0 steps";
                return false;
            }

            error = null;
            return true;
        }
    }
}

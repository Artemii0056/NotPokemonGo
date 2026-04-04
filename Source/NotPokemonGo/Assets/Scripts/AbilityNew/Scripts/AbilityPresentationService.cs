using System.Collections.Generic;
using AbilityNew.Scripts.Executors;
using AbilityNew.Scripts.Presentation;
using AbilityNew.Scripts.Presentation.AbilityNew.Scripts.Presentation;

namespace AbilityNew.Scripts
{
    public sealed class AbilityPresentationService : IAbilityPresentationService
    {
        private readonly Dictionary<AbilitySo, AbilityPresentationConfig> _configs = new();
        private readonly PresentationStepExecutorRegistry _executorRegistry;

        public AbilityPresentationService(PresentationStepExecutorRegistry executorRegistry)
        {
            _executorRegistry = executorRegistry;
        }

        public void Register(AbilityPresentationConfig config)
        {
            if (config == null || config.Ability == null)
                return;

            _configs[config.Ability] = config;
        }

        public void Play(AbilityPresentationContext context)
        {
            if (context == null || context.Ability == null)
                return;

            if (_configs.TryGetValue(context.Ability, out var config) == false)
                return;

            for (int i = 0; i < config.Entries.Count; i++)
            {
                var entry = config.Entries[i];

                if (entry.Signal != context.Signal)
                    continue;

                ExecuteEntry(entry, context);
            }
        }

        private void ExecuteEntry(PresentationEntry entry, AbilityPresentationContext context)
        {
            if (entry.Cases == null || entry.Cases.Count == 0)
                return;

            for (int i = 0; i < entry.Cases.Count; i++)
            {
                var presentationCase = entry.Cases[i];

                if (presentationCase == null)
                    continue;

                if (presentationCase.Condition != null &&
                    presentationCase.Condition.Evaluate(context) == false)
                    continue;

                ExecuteSteps(presentationCase.Steps, context);
                break;
            }
        }

        private void ExecuteSteps(List<PresentationStep> steps, AbilityPresentationContext context)
        {
            if (steps == null)
                return;

            for (int i = 0; i < steps.Count; i++)
                _executorRegistry.Execute(steps[i], context);
        }
    }
}
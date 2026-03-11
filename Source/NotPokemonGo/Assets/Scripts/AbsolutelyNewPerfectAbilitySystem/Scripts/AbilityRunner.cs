using Abilities.Runtime;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts
{
    public class AbilityRunner
    {
        private StepExecutorRegistry _executors;

        public AbilityRunner(StepExecutorRegistry executors) => 
            _executors = executors;

        public async UniTask RunAbility(AbilitySO ability, AbilityContext ctx)
        {
            foreach (var step in ability.Steps)
            {
                await _executors.Execute(step, ctx);
            }
        }
    }
}
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Configs;
using AbsolutelyNewPerfectAbilitySystem.Steps;
using Cysharp.Threading.Tasks;
using Units.Movement;

namespace AbsolutelyNewPerfectAbilitySystem.Executors
{
    public class MoveExecutor : IAbilityStepExecutor
    {
        private readonly IUnitMover _unitMover;

        public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var data = (MoveStepSO)step;

            await _unitMover.MoveTo(
                ctx.Source.transform,
                ctx.Target.transform.position,
                data.Speed
            );
        }
    }
}
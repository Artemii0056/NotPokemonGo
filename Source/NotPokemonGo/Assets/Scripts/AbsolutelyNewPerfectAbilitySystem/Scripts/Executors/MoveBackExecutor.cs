using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using Cysharp.Threading.Tasks;
using Units.Movement;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class MoveBackExecutor : IAbilityStepExecutor
    {
        private readonly IUnitMover _unitMover;

        public MoveBackExecutor(IUnitMover unitMover) => 
            _unitMover = unitMover;

        public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var data = (MoveBackStep)step;

            await _unitMover.MoveTo(
                ctx.Source.transform,
                ctx.StartPosition,
                data.Speed
            );
        }
    }
}
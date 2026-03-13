using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using Units.Movement;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public class MoveExecutor : AbilityStepExecutor<MoveStep>
    {
        private readonly IUnitMover _unitMover;
        private DistanceCalculator _calculator;

        public MoveExecutor(IUnitMover unitMover)
        {
            _calculator = new DistanceCalculator(); //TODO Вынести в сервис
            _unitMover = unitMover;
        }

        public override async UniTask Execute(MoveStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            AbilityExecutionContext context = runtime.Context;

            await _unitMover.MoveTo(
                context.Source.transform,
                _calculator.CalculateDistance(context.Source, context.Target),
                step.Speed
            );
        }
    }
}
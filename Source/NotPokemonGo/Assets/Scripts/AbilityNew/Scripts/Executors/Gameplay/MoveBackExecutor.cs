using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using Units.Movement;
using UnityEngine;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public class MoveBackExecutor : AbilityStepExecutor<MoveBackStep>
    {
        private readonly IUnitMover _unitMover;

        public MoveBackExecutor(IUnitMover unitMover) => 
            _unitMover = unitMover;

        public  override async UniTask Execute(MoveBackStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            Debug.Log("MoveBackExecutor");
            
            await _unitMover.MoveTo(
                runtime.Context.Source.transform,
                runtime.Context.Source.StartPosition,
                step.Speed
            );
        }
    }
}
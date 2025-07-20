using System.Collections;
using Infrastructure.StateMachines.States.Interfaces;
using Services;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class UpdateBattleTickState : IPayloadedState<Battlefield>
    {
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;

        public UpdateBattleTickState(IBattleStateMachine battleStateMachine, ICoroutineRunner coroutineRunner)
        {
            _battleStateMachine = battleStateMachine;
            _coroutineRunner = coroutineRunner;
        }
        
        public void Enter(Battlefield unitActionPayload)
        {
            unitActionPayload.Tick();
            _coroutineRunner.StartCoroutine(Delay(unitActionPayload));
        }

        private IEnumerator Delay(Battlefield battlefield)
        {
            yield return new WaitForSeconds(0.1f);
            _battleStateMachine.Enter<SelectReadyUnitState, Battlefield>(battlefield);
        }

        public void Exit()
        {
            
        }
    }
}
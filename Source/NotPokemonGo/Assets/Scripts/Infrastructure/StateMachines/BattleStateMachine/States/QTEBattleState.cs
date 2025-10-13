using Infrastructure.StateMachines.States.Interfaces;
using Services.QTEServices;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class QTEBattleState : IPayloadedState<Battlefield>
    {
        private readonly IQTEService _qteService;
        private readonly IBattleStateMachine _battleStateMachine;
        private Battlefield _battlefield;

        public QTEBattleState(IQTEService qteService, IBattleStateMachine battleStateMachine)
        {
            _qteService = qteService;
            _battleStateMachine = battleStateMachine;
        }

        public void Enter(Battlefield battlefield)
        {
            _battlefield = battlefield;
            _qteService.Start();
            _qteService.Completed += OnCompleted;
        }

        public void Exit()
        {
            _qteService.Completed -= OnCompleted;
        }

        private void OnCompleted(bool isSuccess)
        {
            if (isSuccess)
            {
                Debug.Log("QTEBattleState::OnCompleted");
            }
            else
            {
                Debug.Log("QTEBattleState::OnFailed");
            }
            
            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(_battlefield);
        }
    }
}
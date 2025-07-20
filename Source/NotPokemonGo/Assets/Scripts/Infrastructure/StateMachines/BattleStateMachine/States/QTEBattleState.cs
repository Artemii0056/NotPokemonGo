using Infrastructure.StateMachines.States.Interfaces;
using Services.QTEServices;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class QTEBattleState : IState
    {
        private readonly IQTEService _qteService;

        public QTEBattleState(IQTEService qteService)
        {
            _qteService = qteService;
        }
        
        public void Enter()
        {
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
        }
    }
}
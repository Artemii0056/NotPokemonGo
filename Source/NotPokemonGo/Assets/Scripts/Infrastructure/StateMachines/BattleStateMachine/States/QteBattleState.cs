using Abilities;
using Battlefields;
using Infrastructure.StateMachines.States.Interfaces;
using Services.QTEServices;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class QteBattleState : IPayloadedState<QTEPayload> //TODO DELETE
    {
        private readonly IQteService _qteService;
        private readonly IBattleStateMachine _battleStateMachine;
        private Battlefield _battlefield;

        public QteBattleState(IQteService qteService, IBattleStateMachine battleStateMachine)
        {
            _qteService = qteService;
            _battleStateMachine = battleStateMachine;
        }

        public void Enter(QTEPayload qtePayload)
        {
            _battlefield = qtePayload.Battlefield;
            //_qteService.Start(qtePayload.AbilityType);
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
    
    public class QTEPayload
    {
        public Battlefield Battlefield { get; set; }
        public AbilityType AbilityType { get; set; }
    }
}
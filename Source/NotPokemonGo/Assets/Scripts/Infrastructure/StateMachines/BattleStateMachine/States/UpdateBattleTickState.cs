using Infrastructure.StateMachines.States.Interfaces;
using UI.Ability;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class UpdateBattleTickState : IPayloadedState<Battlefield>
    {
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly AbilityPanelPresenter _abilityPanelPresenter;

        public UpdateBattleTickState(IBattleStateMachine battleStateMachine, AbilityPanelPresenter abilityPanelPresenter)
        {
            _abilityPanelPresenter = abilityPanelPresenter;
            _battleStateMachine = battleStateMachine;
        }
        
        public void Enter(Battlefield battlefield)
        {
            _abilityPanelPresenter.Disable();
            battlefield.Tick(5f);
            _battleStateMachine.Enter<SelectReadyUnitState, Battlefield>(battlefield);
        }

        public void Exit()
        {
            
        }
    }
}
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.States.Interfaces;
using UI.Ability;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class BattleLoopState : IPayloadedState<Battlefield>
    {
        private readonly ITargetSelector _targetSelector;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IBattleUnitContainer _battleUnitContainer;
        private readonly AbilityPanelPresenter _abilityPanelPresenter;

        private Battlefield _battlefield;

        public BattleLoopState(
            ITargetSelector targetSelector,
            IBattleStateMachine battleStateMachine,
            IBattleUnitContainer battleUnitContainer,
            AbilityPanelPresenter abilityPanelPresenter
            )
        {
            _targetSelector = targetSelector;
            _battleStateMachine = battleStateMachine;
            _battleUnitContainer = battleUnitContainer;
            _abilityPanelPresenter = abilityPanelPresenter;
        }

        public void Enter(Battlefield unitActionPayload)
        {
            _abilityPanelPresenter.Disable();
            _battleUnitContainer.Reset();
            _battlefield = unitActionPayload;
            _battlefield.Enable();

            _targetSelector.SetPlatoons(_battlefield.EnemyPlatoon, _battlefield.Heroes);
            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(_battlefield);
        }

        public void Exit()
        {
            _battlefield.Disable();
        }
    }
}
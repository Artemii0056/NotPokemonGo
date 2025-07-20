using Infrastructure.StateMachines.BattleStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using UI.BattleUpgrages;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class FinishBattleState : IPayloadedState<UnitActionPayload>
    {
        private readonly BattleUpgradePanelPresenter _battleUpgradePanelPresenter;
        private readonly IBattleStateMachine _battleStateMachine;
        private UnitActionPayload _unitActionPayload;

        public FinishBattleState(IBattleStateMachine battleStateMachine, BattleUpgradePanelPresenter battleUpgradePanelPresenter)
        {
            _battleStateMachine = battleStateMachine;
            _battleUpgradePanelPresenter = battleUpgradePanelPresenter;
        }
        
        public void Enter(UnitActionPayload unitActionPayload)
        {
            _unitActionPayload = unitActionPayload;
            _battleUpgradePanelPresenter.Enable(unitActionPayload.UnitSorce.UnitType);

            _battleUpgradePanelPresenter.UpgradeSelected += OnUpgradeSelected;
        }

        private void OnUpgradeSelected()
        {
            _battleStateMachine.Enter<UnitActionState, UnitActionPayload>(_unitActionPayload);
        }

        public void Exit()
        {
            _battleUpgradePanelPresenter.UpgradeSelected -= OnUpgradeSelected;
            _battleUpgradePanelPresenter.Disable();
        }
    }
}
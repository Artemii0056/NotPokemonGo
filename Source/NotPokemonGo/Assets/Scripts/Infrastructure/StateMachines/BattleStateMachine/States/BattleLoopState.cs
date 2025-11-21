using Battlefields;
using Infrastructure.StateMachines.States.Interfaces;
using UI.Ability;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class BattleLoopState : IPayloadedState<Battlefield>
    {
        private readonly ITargetSelector _targetSelector;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly AbilityPanelPresenter _abilityPanelPresenter;

        private Battlefield _battlefield;

        public BattleLoopState(
            ITargetSelector targetSelector,
            IBattleStateMachine battleStateMachine,
            AbilityPanelPresenter abilityPanelPresenter
            )
        {
            _targetSelector = targetSelector;
            _battleStateMachine = battleStateMachine;
            _abilityPanelPresenter = abilityPanelPresenter;
        }

        public void Enter(Battlefield battlefield)
        {
            _abilityPanelPresenter.Disable();
            _battlefield = battlefield;

            _targetSelector.SetPlatoons(_battlefield.EnemyPlatoon, _battlefield.HeroesPlatoon);
            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(_battlefield);
        }

        public void Exit()
        {
        }
    }
}
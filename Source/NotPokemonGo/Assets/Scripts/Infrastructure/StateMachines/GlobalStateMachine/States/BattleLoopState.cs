using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.States.Interfaces;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class BattleLoopState : IPayloadedState<Battlefield>
    {
        private readonly ITargetSelector _targetSelector;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IBattleUnitContainer _battleUnitContainer;

        private Battlefield _battlefield;

        public BattleLoopState(ITargetSelector targetSelector, IBattleStateMachine battleStateMachine, IBattleUnitContainer battleUnitContainer)
        {
            _targetSelector = targetSelector;
            _battleStateMachine = battleStateMachine;
            _battleUnitContainer = battleUnitContainer;
        }

        public void Enter(Battlefield battlefield)
        {
            _battleUnitContainer.Reset();
            _battlefield = battlefield;
            _battlefield.Enable();

            _targetSelector.SetPlatoons(_battlefield.EnemyPlatoon, _battlefield.Platoon2);
            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(_battlefield);
        }

        public void Exit()
        {
            _battlefield.Disable();
        }
    }
}
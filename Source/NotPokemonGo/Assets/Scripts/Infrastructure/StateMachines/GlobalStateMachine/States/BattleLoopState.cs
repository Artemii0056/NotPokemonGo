using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class BattleLoopState : IPayloadedState<BattleLoopPayload>
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

        public void Enter(BattleLoopPayload payload)
        {
            _battleUnitContainer.Reset();
            _battlefield = payload.Battlefield;
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
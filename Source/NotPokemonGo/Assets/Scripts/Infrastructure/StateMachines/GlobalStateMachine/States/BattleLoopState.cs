using Infrastructure.StateMachines.GlobalStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class BattleLoopState : IUpdateState, IPayloadedState<BattleLoopPayload>
    {
        private Battlefield _battlefield;
        private ITargetSelector _targetSelector;

        public BattleLoopState(ITargetSelector targetSelector)
        {
            _targetSelector = targetSelector;
        }

        public void Enter(BattleLoopPayload payload)
        {
            _battlefield = payload.Battlefield;
            _battlefield.Enable();

            _targetSelector.SetPlatoons(_battlefield.EnemyPlatoon, _battlefield.Platoon2);
        }
        
        public void Update(float deltaTime)
        {
            _battlefield?.Tick(deltaTime);
        }

        public void Exit()
        {
            _battlefield.Disable();
        }
    }
}
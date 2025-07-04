using Infrastructure.StateMachines.States.Interfaces;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class UpdateBattleTickState : IPayloadedState<Battlefield>
    {
        private readonly IBattleStateMachine _battleStateMachine;

        public UpdateBattleTickState(IBattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
        }
        
        public void Enter(Battlefield battlefield)
        {
            battlefield.Tick();
            //Debug.LogError("Разберись с StatusType и EffectType. эффект = урон и хил. Статусы = яд, благословение пизды, увольнение Сени и пр.");
            _battleStateMachine.Enter<SelectReadyUnitState, Battlefield>(battlefield);
        }

        public void Exit()
        {
            
        }
    }
}
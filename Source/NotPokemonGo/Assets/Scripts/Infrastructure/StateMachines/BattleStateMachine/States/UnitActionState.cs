using System;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using VContainer;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class UnitActionState : IPayloadedState<UnitActionPayload>
    {
        private readonly IObjectResolver _objectResolver;
        
        private UnitActionStrategy _unitActionStrategy;

        public UnitActionState(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        
        public void Enter(UnitActionPayload payload)
        {
            switch (payload.UnitSorce.PlatoonType)
            {
                case PlatoonType.Friends:
                    _unitActionStrategy = new FriendUnitActionStrategy(payload.Battlefield, payload.UnitSorce);
                    break;

                case PlatoonType.Enemies:
                    _unitActionStrategy = new EnemyUnitActionStrategy(payload.Battlefield, payload.UnitSorce);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _objectResolver.Inject(_unitActionStrategy);

            _unitActionStrategy.Enable();        
        }

        public void Exit()
        {
            _unitActionStrategy.Disable();
        }
    }
}
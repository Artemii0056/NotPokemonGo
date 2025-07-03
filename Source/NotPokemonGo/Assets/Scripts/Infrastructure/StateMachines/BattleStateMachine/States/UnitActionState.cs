using System;
using Animations;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class UnitActionState : IPayloadedState<UnitActionPayload>
    {
        private readonly IObjectResolver _objectResolver;
        private readonly IAnimationProcessingService _animationProcessingService;

        private UnitActionStrategy _unitActionStrategy;

        public UnitActionState(IObjectResolver objectResolver, IAnimationProcessingService animationProcessingService)
        {
            _objectResolver = objectResolver;
            _animationProcessingService = animationProcessingService;
        }
        
        public void Enter(UnitActionPayload payload)
        {
            switch (payload.UnitSorce.PlatoonType)
            {
                case PlatoonType.Friends:
                    _unitActionStrategy = new FriendUnitActionStrategy(payload.Battlefield, payload.UnitSorce);
                    break;

                case PlatoonType.Enemies:
                    _unitActionStrategy = new EnemyUnitActionStrategy(payload.Battlefield, payload.UnitSorce, _animationProcessingService);
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
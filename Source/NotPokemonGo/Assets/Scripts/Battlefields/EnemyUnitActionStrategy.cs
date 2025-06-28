using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using UnityEngine;
using VContainer;

namespace Battlefields
{
    public class EnemyUnitActionStrategy : UnitActionStrategy
    {
        private readonly Battlefield _battlefield;
        
        private IBattleStateMachine _battleStateMachine;

        public EnemyUnitActionStrategy(Battlefield battlefield)
        {
            _battlefield = battlefield;
        }

        [Inject]
        public void Initialize(IBattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
        }
        
        public override void Enable()
        {
            base.Enable();
            Debug.Log("Враг походил");
            _battleStateMachine.Enter<UpdateBattleTickState>();
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}
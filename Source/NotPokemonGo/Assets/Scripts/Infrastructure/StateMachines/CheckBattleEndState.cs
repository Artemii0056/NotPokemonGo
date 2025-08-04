using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Infrastructure.StateMachines.States.Interfaces;
using Services;
using UI.Factory;
using UnityEngine;

namespace Infrastructure.StateMachines
{
    public class CheckBattleEndState : IPayloadedState<Battlefield> //Это Батя. Который закончит игру, если все пройдено 
    {
        private IGameStateMachine _gameStateMachine;
        private IBattleStateMachine _battleStateMachine;
        private IUIFactory _uiFactory;
        private Battlefield _battlefield;

        public CheckBattleEndState(
            IGameStateMachine gameStateMachine,
            IBattleStateMachine battleStateMachine, 
            IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _battleStateMachine = battleStateMachine;
            _uiFactory = uiFactory;
        }

        public void Enter(Battlefield battlefield)
        {
            _battlefield = battlefield;
            
            if (battlefield.EnemyPlatoon.HaveUnits == false && battlefield.HeroesPlatoon.HaveUnits == false)
            {
                Debug.Log("Сделать авто проигрыш");
            }

            if (battlefield.EnemyPlatoon.HaveUnits == false)
            {
                _gameStateMachine.Enter<WaveProgressionState>();
                Debug.Log("Enter new wave");
                return;
            }

            if (battlefield.HeroesPlatoon.HaveUnits == false)
            {
                LoosePanel loosePanel = _uiFactory.CreateLoosePanel();
                _battleStateMachine.Enter<LoosePanelState, LoosePanel>(loosePanel); //Чет тут херня
                Debug.Log("Heroes.HaveUnitsDead");
                return;
            }

            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(battlefield);
        }

        public void Exit()
        {
        }
    }
}
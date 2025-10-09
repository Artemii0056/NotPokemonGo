using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Infrastructure.StateMachines.States.Interfaces;
using Platoons;
using Services.BattleSessionService;
using UI.Factory;
using UnityEngine;

namespace Infrastructure.StateMachines
{
    public class
        CheckBattleEndState : IPayloadedState<Battlefield>
    {
        private IGameStateMachine _gameStateMachine;
        private IBattleStateMachine _battleStateMachine;
        private IUIFactory _uiFactory;
        private IBattlefieldSessionService _battlefieldSessionService;

        public CheckBattleEndState(
            IGameStateMachine gameStateMachine,
            IBattleStateMachine battleStateMachine,
            IUIFactory uiFactory,
            IBattlefieldSessionService battlefieldSessionService)
        {
            _gameStateMachine = gameStateMachine;
            _battleStateMachine = battleStateMachine;
            _uiFactory = uiFactory;
            _battlefieldSessionService = battlefieldSessionService;
        }

        public void Enter(Battlefield battlefield)
        {
            bool heroesDead = !battlefield.HeroesPlatoon.HaveUnits;
            bool enemiesDead = !battlefield.EnemyPlatoon.HaveUnits;

            if (heroesDead && enemiesDead)
            {
                Debug.Log("Сделать авто проигрыш");
            }

            if (enemiesDead) //Вот тут добавить логику у WaveProgressionState с новым Входом, принимающим список текущих бойцов
            {
               // var survivor = 
                
                // Platoon platoon = new Platoon(_battlefield.HeroesPlatoon.AliveUnits, _battlefield.HeroesPlatoon.Type);
                // battlefield.Disable();
                // battlefield.DiscardAll();
                //
                // Debug.Log(platoon.AliveUnits.Count);
                //
                // _gameStateMachine.Enter<WaveProgressionState, Platoon>(platoon);
                Debug.Log("Enter new wave");
                
                var survivors = battlefield.HeroesPlatoon.AliveUnits;
                _battlefieldSessionService.Cleanup();

                Platoon platoon = new Platoon(survivors, battlefield.HeroesPlatoon.Type);

                _gameStateMachine.Enter<WaveProgressionState, Platoon>(platoon);
                return;
            }

            if (heroesDead)
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
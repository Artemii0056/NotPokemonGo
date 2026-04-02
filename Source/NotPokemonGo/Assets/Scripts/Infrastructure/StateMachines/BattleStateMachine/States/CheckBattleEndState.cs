using Battlefields;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.States.Interfaces;
using Platoons;
using Services.BattleSessionService;
using UI;
using UI.Factory;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class CheckBattleEndState : IPayloadedState<Battlefield>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IBattlefieldSessionService _battlefieldSessionService;

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
            //Debug.Log("CheckBattleEndState ENTER");
            
            bool heroesDead = !battlefield.HeroesPlatoon.HaveUnits;
            bool enemiesDead = !battlefield.EnemyPlatoon.HaveUnits;
            
           // Debug.Log($"CheckBattleEndState heroesDead={heroesDead}, enemiesDead={enemiesDead}");

            if (heroesDead && enemiesDead)
            {
               //Debug.Log("Сделать авто проигрыш");
            }

            if (enemiesDead) 
            {
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
               // Debug.Log("Heroes.HaveUnitsDead");
                return;
            }

            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(battlefield);
        }

        public void Exit()
        {
        }
    }
}
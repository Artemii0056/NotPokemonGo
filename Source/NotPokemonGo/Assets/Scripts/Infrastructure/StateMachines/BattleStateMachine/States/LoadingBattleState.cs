using System.Collections.Generic;
using Characters.Configs;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Infrastructure.StateMachines.States.Interfaces;
using Services;
using Services.SceneServices;
using UI.Factory;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class LoadingBattleState : IPayloadedState<LoadingBattleStatePayload>, IState 
    {
        private readonly ILevelProgressService _levelProgressService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        
        private LoadingBattleStatePayload _payload;
        private readonly ISceneLoader _sceneLoader;

        public LoadingBattleState(
            IGameStateMachine gameStateMachine,
            ILevelProgressService levelProgressService, 
            ISceneLoader sceneLoader, 
            IUIFactory uiFactory)
        {
            _levelProgressService = levelProgressService;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(LoadingBattleStatePayload battlefield)
        {
            BattleInfoUI battleUIInfo = _uiFactory.CreateBattleUIInfo();
            
            _payload = battlefield;
             
             List<UnitType> units = battlefield.UnitTypes;
            
            LevelRuntimeDataPayload levelRuntimeDataPayload = 
                new LevelRuntimeDataPayload(battlefield.LevelConfig, units, battleUIInfo);
            
            _levelProgressService.Set(levelRuntimeDataPayload); //Надо дропнуть _levelProgressService
            
            _sceneLoader.Load(Constants.AssetPath.MainMenuSceneName, EnterGlobalBattleState);
        }

        private void EnterGlobalBattleState()
        {
            BattleInfoUI battleUIInfo = _uiFactory.CreateBattleUIInfo();
            
            List<UnitType> units = _payload.UnitTypes;
            
            LevelRuntimeDataPayload levelRuntimeDataPayload = 
                new LevelRuntimeDataPayload(_payload.LevelConfig, units, battleUIInfo);
            
            _levelProgressService.Set(levelRuntimeDataPayload);

            _gameStateMachine.Enter<GlobalBattleState, LevelRuntimeDataPayload>(levelRuntimeDataPayload);
        }

        public void Enter() 
        {
            BattleInfoUI battleInfoUI = Object.FindObjectOfType<BattleInfoUI>(); 
            battleInfoUI.SetValue(0);
            
            List<UnitType> units = _payload.UnitTypes;
            
            LevelRuntimeDataPayload levelRuntimeDataPayload = new LevelRuntimeDataPayload(_payload.LevelConfig, units, battleInfoUI);
            
            _levelProgressService.Set(levelRuntimeDataPayload);
            
            //_gameStateMachine.Enter<WaveProgressionState>();
        }
        
        public void Exit()
        {
        }
    }
}
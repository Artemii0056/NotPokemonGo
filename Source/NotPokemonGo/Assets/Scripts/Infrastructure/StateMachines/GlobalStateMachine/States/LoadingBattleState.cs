using System.Collections.Generic;
using Characters.Configs;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.States.Interfaces;
using Services;
using Services.BattleUnitContainers;
using Services.SceneServices;
using Services.StaticDataServices;
using UI.Factory;
using UnityEngine;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class LoadingBattleState : IPayloadedState<LoadingBattleStatePayload>, IState //Сюда передать UI и менять его значение в WavePross
    //После загрузки нужно в Баттл стейт перейти 
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IBattlefieldFactory _battlefieldFactory;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IUIFactory _uiFactory;
        
        private LoadingBattleStatePayload _payload;
        private readonly ISceneLoader _sceneLoader;

        public LoadingBattleState(
            IGameStateMachine gameStateMachine,
            IStaticDataService staticDataService,
            IBattlefieldFactory battlefieldFactory, 
            ILevelProgressService levelProgressService, ISceneLoader sceneLoader, 
            IBattleStateMachine battleStateMachine, IUIFactory uiFactory)
        {
            _battlefieldFactory = battlefieldFactory;
            _levelProgressService = levelProgressService;
            _sceneLoader = sceneLoader;
            _battleStateMachine = battleStateMachine;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(LoadingBattleStatePayload levelData)
        {
            BattleInfoUI battleUIInfo = _uiFactory.CreateBattleUIInfo();
            
            Debug.Log("LoadingBattleState");
            
            _payload = levelData;
             
             List<UnitType> units = levelData.UnitTypes;
            
            LevelRuntimeDataPayload levelRuntimeDataPayload = 
                new LevelRuntimeDataPayload(levelData.LevelConfig, units, battleUIInfo);
            
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
            
            Debug.Log("Simple LoadingBattleState");
            
            List<UnitType> units = _payload.UnitTypes;
            
            LevelRuntimeDataPayload levelRuntimeDataPayload = new LevelRuntimeDataPayload(_payload.LevelConfig, units, battleInfoUI);
            
            _levelProgressService.Set(levelRuntimeDataPayload);
            
            _gameStateMachine.Enter<WaveProgressionState>();
        }
        
        public void Exit()
        {
        }
    }
}
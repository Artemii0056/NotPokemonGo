using System.Collections.Generic;
using Infrastructure.StateMachines.States.Interfaces;
using Map;
using PersistentProgresses;
using SaveLoadService;
using Services.SceneServices;
using UI;
using UI.Factory;
using UnityEngine;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class BootstrapState : IState, IProgressReader
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;

        public BootstrapState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, IUIFactory uiFactory, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            _saveLoadService.ProgressReaders.Add(this);
            _saveLoadService.LoadProgress();
        }

        private void EnterMainMenuState()
        {
            StartScreenUI screenUI = _uiFactory.CreateStartScreen();
            
            StartMenuPayload payload = new StartMenuPayload(screenUI);
            _gameStateMachine.Enter<StartScreenState, StartMenuPayload>(payload); 
        }

        public void Exit()
        {
        }

        public void ReadProgress(ProjectProgress projectProgress)
        {
            Dictionary<MapType, int> mapsCompleted = projectProgress.PlayerProgress.LevelProgress.StartMapsCompleted;

            if (mapsCompleted.Keys.Count == 0)
            {
                Debug.Log("нет прохождений карт");
            }
            else
            {
                foreach (MapType mapType in mapsCompleted.Keys)
                {
                    Debug.Log($"карта {mapType} была пройдена = {mapsCompleted[mapType]}");
                }
            }
            
            _sceneLoader.Load(Constants.AssetPath.CharacterSelectionSceneName, EnterMainMenuState);
        }
    }
}
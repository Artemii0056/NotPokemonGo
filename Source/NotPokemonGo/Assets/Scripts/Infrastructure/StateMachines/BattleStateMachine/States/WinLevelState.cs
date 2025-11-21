using System.Collections.Generic;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Infrastructure.StateMachines.States.Interfaces;
using LevelSetting;
using Map;
using PersistentProgresses;
using SaveLoadService;
using Services.StaticDataServices;
using UI.Factory;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
  public class WinLevelState : IState, IProgressWriter
  {
    private readonly ISaveLoadService _saveLoadService;
    private readonly ICurrentMapTypeProvider _currentMapTypeProvider;
    private readonly IStaticDataService _staticDataService;
    private readonly IUIFactory _uiFactory;
    private readonly IGameStateMachine _gameStateMachine;

    private MapType _mapType;
    private WinPanel _winPanel;

    public WinLevelState(
      ISaveLoadService saveLoadService, 
      ICurrentMapTypeProvider currentMapTypeProvider, 
      IStaticDataService staticDataService, 
      IUIFactory uiFactory,
      IGameStateMachine gameStateMachine)
    {
      _saveLoadService = saveLoadService;
      _currentMapTypeProvider = currentMapTypeProvider;
      _staticDataService = staticDataService;
      _uiFactory = uiFactory;
      _gameStateMachine = gameStateMachine;
    }
        
    public void Enter()
    {
      _winPanel = _uiFactory.CreateWinPanel();
      _winPanel.BackMainMenuButtonPressed += OnBackMainMenuButtonPressed;
      _winPanel.RestartButtonPressed += OnRestartButtonPressed;
      // LevelConfig levelConfig = _staticDataService.GetLevelConfig(_currentMapTypeProvider.CurrentMapType);
      // _mapType =  levelConfig.MapType;
      _saveLoadService.ProgressReaders.Add(this);
    }

    public void Exit()
    {
      _winPanel.BackMainMenuButtonPressed -= OnBackMainMenuButtonPressed;
      _winPanel.RestartButtonPressed -= OnRestartButtonPressed;
    }
    
    public void ReadProgress(ProjectProgress projectProgress)
    {
            
    }

    public void WriteProgress(ProjectProgress projectProgress)
    {
      Dictionary<MapType, int> mapsCompleted = projectProgress.PlayerProgress.LevelProgress.StartMapsCompleted;

      if (mapsCompleted.TryGetValue(_mapType, out int count))
        mapsCompleted[_mapType] = count + 1;
      else
        mapsCompleted[_mapType] = 1;
    }
    
    private void OnRestartButtonPressed()
    {
      _gameStateMachine.Enter<LoadingBattleState>();
    }

    private void OnBackMainMenuButtonPressed()
    {
      _saveLoadService.SaveProgress();

      Object.Destroy(_winPanel.gameObject);
      _gameStateMachine.Enter<BootstrapState>();
    }
  }
}
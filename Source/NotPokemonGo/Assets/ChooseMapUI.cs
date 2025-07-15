using System;
using System.Collections.Generic;
using LevelSetting;
using Services.StaticDataServices;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ChooseMapUI : MonoBehaviour
{
    [SerializeField] private Button _firstButton;
    [SerializeField] private Button _secondButton;

    private List<MapLevel> _mapLevels;
    private IStaticDataService _staticDataService;

    public event Action MapExitRequested; 

    public void Initialize(List<MapLevel> mapLevels, IStaticDataService staticDataService, UnitSelectionController unitSelectionController)
    {
        _mapLevels = mapLevels;
        _staticDataService = staticDataService;

        List<LevelConfig> levelConfigs = _staticDataService.GetLevelConfigs();

        for (int i = 0; i < _mapLevels.Count; i++)
        {
            _mapLevels[i].Initialize(levelConfigs);
            _mapLevels[i].Set(unitSelectionController);
            _mapLevels[i].ExitButtonClicked += OnMapExitClicked; 
        }
    }

    private void OnEnable()
    {
        _firstButton.onClick.AddListener(OnFirstButtonClicked);
    }

    private void OnDisable()
    {
        _firstButton.onClick.RemoveListener(OnFirstButtonClicked);

        foreach (var level in _mapLevels)
            level.ExitButtonClicked -= OnMapExitClicked;
    }

    private void OnFirstButtonClicked()
    {
        gameObject.SetActive(false);
        _mapLevels[0].gameObject.SetActive(true);
    }

    private void OnMapExitClicked()
    {
        foreach (var level in _mapLevels)
            level.gameObject.SetActive(false);

        gameObject.SetActive(true);
        MapExitRequested?.Invoke(); 
    }
}
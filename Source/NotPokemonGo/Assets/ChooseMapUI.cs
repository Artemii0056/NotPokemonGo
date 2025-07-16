using System;
using System.Collections.Generic;
using LevelSetting;
using Map;
using Services.StaticDataServices;
using UI;
using UnityEngine;

public class ChooseMapUI : MonoBehaviour 
{
    [SerializeField] private List<MapButton> _mapButtons;

    public UnitSelectionController UnitSelectionController { get; private set; }

    public List<MapLevel> MapLevels { get; private set; }
    private IStaticDataService _staticDataService;

    public event Action<MapType> MapChoosed;

    public void Initialize(List<MapLevel> mapLevels, IStaticDataService staticDataService,
        UnitSelectionController unitSelectionController)
    {
        UnitSelectionController = unitSelectionController;
        MapLevels = mapLevels;

        _staticDataService = staticDataService;

        List<LevelConfig> levelConfigs = _staticDataService.GetLevelConfigs();

        for (int i = 0; i < MapLevels.Count; i++)
        {
            MapLevels[i].Initialize(levelConfigs);
        }
    }

    private void OnEnable()
    {
        foreach (var button in _mapButtons)
            button.OnClick += OnButtonClick;
    }

    private void OnButtonClick(MapType type) => 
        MapChoosed?.Invoke(type);

    private void OnDisable()
    {
        foreach (var button in _mapButtons)
            button.OnClick -= OnButtonClick;
    }
}
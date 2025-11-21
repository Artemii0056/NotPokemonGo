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

    private ICurrentMapTypeProvider _currentMapTypeProvider;

    public ChooseUnitToFightPanel ChooseUnitToFightPanel { get; private set; }

    public List<MapLevel> MapLevels { get; private set; }

    public event Action<MapType> MapSelected;

    public void Initialize(List<MapLevel> mapLevels, 
        IStaticDataService staticDataService,
        ChooseUnitToFightPanel chooseUnitToFightPanel,
        ICurrentMapTypeProvider currentMapTypeProvider)
    {
        _currentMapTypeProvider = currentMapTypeProvider;
        ChooseUnitToFightPanel = chooseUnitToFightPanel;
        MapLevels = mapLevels;

        List<LevelConfig> levelConfigs = staticDataService.GetLevelConfigs();

        for (int i = 0; i < MapLevels.Count; i++) 
            MapLevels[i].Initialize(levelConfigs);
    }

    private void OnEnable()
    {
        foreach (var button in _mapButtons)
            button.OnClick += OnButtonClick;
    }

    private void OnDisable()
    {
        foreach (var button in _mapButtons)
            button.OnClick -= OnButtonClick;
    }
    
    private void OnButtonClick(MapType type)
    {
        _currentMapTypeProvider.CurrentMapType = type;
        MapSelected?.Invoke(type);
    }
}
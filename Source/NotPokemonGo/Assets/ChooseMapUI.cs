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

    private List<LevelConfig> _levelConfig;

    private List<MapLevel> _mapLevels;

    public void Initialize(List<MapLevel> mapLevels, IStaticDataService staticDataService)
    {
        _mapLevels = mapLevels;
        
       List<LevelConfig> levelConfigs = staticDataService.GetLevelConfigs();

        for (int i = 0; i < _mapLevels.Count; i++)
        {
            _mapLevels[i].Initialize(levelConfigs);
        }
    }

    private void OnEnable()
    {
        _firstButton.onClick.AddListener(OnFirstButtonClicked);
      //  _secondButton.onClick.AddListener(OnSecondButtonClicked);
    }

    public void OnDisable()
    {
        _firstButton.onClick.RemoveListener(OnFirstButtonClicked);
        //_secondButton.onClick.RemoveListener(OnSecondButtonClicked);
    }

    private void OnFirstButtonClicked()
    {
        gameObject.SetActive(false);
        _mapLevels[0].gameObject.SetActive(true);
    }

    // private void OnSecondButtonClicked()
    // {
    //     gameObject.SetActive(false);
    //     _mapLevels[1].gameObject.SetActive(true);
    // }
}
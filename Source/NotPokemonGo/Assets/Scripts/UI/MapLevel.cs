using System;
using System.Collections.Generic;
using LevelSetting;
using Map;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _playButton;

        [field: SerializeField] public MapType MapType { get; private set; }

        [SerializeField] private List<LevelButton> _buttons;

        private Dictionary<MapType, LevelConfig> _configs = new Dictionary<MapType, LevelConfig>();
        private LevelConfig _currentLevelConfig;

        private UnitSelectionController _unitSelectionController;

        public event Action OnPlayButtonClicked;
        
        public event Action ExitButtonClicked;

        public void Initialize(List<LevelConfig> levelConfigs)
        {
            foreach (var levelConfig in levelConfigs)
            {
                _configs.Add(levelConfig.MapType, levelConfig);
            }
        }

        private void OnEnable()
        {
            foreach (var button in _buttons)
                button.OnClick += OnButtonClick;

            _playButton.onClick.AddListener(PlayButtonClick);
            _exitButton.onClick.AddListener(ExitButtonClick);
            _playButton.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            foreach (var button in _buttons)
                button.OnClick -= OnButtonClick;

            _playButton.onClick.RemoveListener(PlayButtonClick);
            _exitButton.onClick.RemoveListener(ExitButtonClick);
        }

        private void ExitButtonClick()
        {
            ExitButtonClicked?.Invoke();
        }

        private void OnButtonClick(MapType mapType)
        {
            LevelConfig info = _configs[mapType];
            _currentLevelConfig = info;

            _playButton.gameObject.SetActive(true);
        }

        private void PlayButtonClick()
        {
            OnPlayButtonClicked?.Invoke();
            
          //  _unitSelectionController.gameObject.SetActive(true);
            //Переход с стейт боя? 
        }

        public void Set(UnitSelectionController unitSelectionController)
        {
            _unitSelectionController = unitSelectionController;
            _unitSelectionController.gameObject.SetActive(false);
        }
    }
}
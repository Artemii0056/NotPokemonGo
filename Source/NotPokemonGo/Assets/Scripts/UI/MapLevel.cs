using System;
using System.Collections.Generic;
using LevelSetting;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Button _exitButton; //Вот это должно работать
        [SerializeField] private Button _playButton;

        [field: SerializeField] public LevelType LevelType { get; private set; }

        [SerializeField] private List<LevelButton> _buttons;

        private Dictionary<LevelType, LevelConfig> _configs = new Dictionary<LevelType, LevelConfig>();
        private LevelConfig _currentLevelConfig;
        
       private UnitSelectionController _unitSelectionController;
       
       public event Action ExitButtonClicked; 

        public void Initialize(List<LevelConfig> levelConfigs)
        {
            foreach (var levelConfig in levelConfigs)
            {
                _configs.Add(levelConfig.LevelType, levelConfig);
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

        private void OnButtonClick(LevelType levelType)
        {
            LevelConfig info = _configs[levelType];
            _currentLevelConfig = info;
            
            _playButton.gameObject.SetActive(true);
        }

        private void PlayButtonClick()
        {
            _unitSelectionController.gameObject.SetActive(true);
            //Переход с стейт боя? 
        }

        public void Set(UnitSelectionController unitSelectionController)
        {
            _unitSelectionController = unitSelectionController;
            _unitSelectionController.gameObject.SetActive(false);
        }
    }
}
using System.Collections.Generic;
using LevelSetting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _playButton;

        [field: SerializeField] public LevelType LevelType { get; private set; }

        [SerializeField] private List<LevelButton> _buttons;

        private Dictionary<LevelType, LevelConfig> _configs = new Dictionary<LevelType, LevelConfig>();
        private LevelConfig _currentLevelConfig;
        
       private ChooseUnitsForBattle _chooseUnitsForBattle;

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
            _playButton.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            foreach (var button in _buttons)
                button.OnClick -= OnButtonClick;
            
            _playButton.onClick.RemoveListener(PlayButtonClick);
        }

        private void OnButtonClick(LevelType levelType)
        {
            LevelConfig info = _configs[levelType];
            _currentLevelConfig = info;
            
            _playButton.gameObject.SetActive(true);

            // for (int i = 0; i < info.LevelParts.Count; i++)
            // {
            //     foreach (var config in info.LevelParts[i].Units)
            //         Debug.Log(config.Type);
            // }
        }

        private void PlayButtonClick()
        {
            _chooseUnitsForBattle.gameObject.SetActive(true);
            //Переход с стейт боя? 
        }

        public void Set(ChooseUnitsForBattle chooseUnitsForBattle)
        {
            _chooseUnitsForBattle = chooseUnitsForBattle;
            _chooseUnitsForBattle.gameObject.SetActive(false);
        }
    }
}
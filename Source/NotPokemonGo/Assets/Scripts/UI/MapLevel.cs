using System;
using System.Collections.Generic;
using LevelSetting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [field: SerializeField] public LevelType LevelType { get; private set; }

        [SerializeField] private List<LevelButton> _buttons;

        private Dictionary<LevelType, LevelConfig> _configs = new Dictionary<LevelType, LevelConfig>();

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
        }

        private void OnDisable()
        {
            foreach (var button in _buttons)
                button.OnClick -= OnButtonClick;
        }

        private void OnButtonClick(LevelType levelType)
        {
            LevelConfig info = _configs[levelType];

            for (int i = 0; i < info.LevelParts.Count; i++)
            {
                Debug.Log(i + 1);

                foreach (var config in info.LevelParts[i].Units)
                    Debug.Log(config.Type);
            }
        }
    }
}
using System;
using Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUpgrages
{
    public class BattleUpgradeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        
        private AbilityType _abilityType;
        public event Action UpgradeSelected;

        public void Initialize(Sprite sprite, AbilityType abilityType)
        {
            _abilityType = abilityType;
            _image.sprite = sprite;
            _text.text = _abilityType.ToString();
            gameObject.SetActive(true);
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
            gameObject.SetActive(false);
        }

        private void OnClick()
        {
            UpgradeSelected?.Invoke();
        }
    }
}
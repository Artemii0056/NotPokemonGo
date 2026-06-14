using System;
using UnityEngine;
using UnityEngine.UI;

namespace Abilities.MV
{
    public class AbilityView : MonoBehaviour
    {
        [field: SerializeField] public Image CooldownImage { get; private set; }

        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        
        private AbilityModel _abilityModel;

        private Image _defaultImage;
        
        public event Action<AbilityModel> OnAbility;

        public void Construct(AbilityModel abilityModel)
        {
            _abilityModel = abilityModel;
            CooldownImage.gameObject.SetActive(true);
        }

        private void Awake() =>
            _defaultImage = GetComponent<Image>();

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            OnAbility?.Invoke(_abilityModel);
        }

        public void SetImage(Sprite sprite) =>
            _icon.sprite = sprite;

        public void SetDefaultImage()
        {
            _icon.sprite = _defaultImage.sprite;
            _abilityModel = null;
        }

        public void Tick(float deltaTime)
        {
            CooldownImage.gameObject.SetActive(false);
            
            if (_abilityModel == null)
                return;

            if (_abilityModel.IsReady())
                return;

            Debug.LogError($"Надо что то думать с заполнением в {typeof(AbilityView)}");
        }
    }
}
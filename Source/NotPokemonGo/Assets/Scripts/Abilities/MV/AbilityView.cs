using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Abilities.MV
{
    public class AbilityView : MonoBehaviour
    {
        [field: SerializeField] public Image CooldownImage { get; private set; }

        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        
        private IAbilityProvider _abilityProvider;

        private AbilityModel _abilityModel;

        private Image _defaultImage;

        [Inject]
        public void Initialize(IAbilityProvider abilityProvider)
        {
            _abilityProvider = abilityProvider;
        }

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
            _abilityProvider.Remember(_abilityModel);
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
            
            // float remaining = _abilityModel.Cooldown - _abilityModel.CurrentTime;
            // float sliderValue = Mathf.Clamp01(remaining / _abilityModel.Cooldown);
            // CooldownImage.fillAmount = sliderValue;
            //
            // if (Mathf.Approximately(sliderValue, 0f))
            // {
            //     CooldownImage.gameObject.SetActive(false);
            // }
            // else
            // {
            //     CooldownImage.gameObject.SetActive(true);
            // }
        }
    }
}
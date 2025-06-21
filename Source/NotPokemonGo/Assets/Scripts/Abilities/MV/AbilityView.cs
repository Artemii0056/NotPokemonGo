using UnityEngine;
using UnityEngine.UI;

namespace Abilities.MV
{
    public class AbilityView : MonoBehaviour
    {
        [field: SerializeField] public Image CooldownImage { get; private set; }

        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        
        private AbilityApplicatorService _abilityApplicatorService;

        private AbilityModel _abilityModel;

        private Image _defaultImage;

        private void Awake() =>
            _defaultImage = GetComponent<Image>();

        public void Initialize(AbilityModel abilityModel)
        {
            _abilityModel = abilityModel;
            CooldownImage.gameObject.SetActive(true);
        }

        private void OnEnable() =>
            _button.onClick.AddListener(OnClick);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnClick);

        public void Update()
        {
            CooldownImage.gameObject.SetActive(false);
            
            if (_abilityModel == null)
                return;

            if (_abilityModel.IsReady)
                return;
            
            float remaining = _abilityModel.Cooldown - _abilityModel.CurrentTime;
            float sliderValue = Mathf.Clamp01(remaining / _abilityModel.Cooldown);
            CooldownImage.fillAmount = sliderValue;

            if (Mathf.Approximately(sliderValue, 0f))
            {
                CooldownImage.gameObject.SetActive(false);
            }
            else
            {
                CooldownImage.gameObject.SetActive(true);
            }
        }

        private void OnClick()
        {
            _abilityApplicatorService.Remember(_abilityModel);
            //_abilityModel.DiscardCurrentTime();
        }

        public void SetImage(Sprite sprite) =>
            _icon.sprite = sprite;

        public void SetDefaultImage()
        {
            _icon.sprite = _defaultImage.sprite;
            _abilityModel = null;
        }

        public void InitService(AbilityApplicatorService abilityApplicatorService)
        {
            _abilityApplicatorService = abilityApplicatorService;
        }
    }
}
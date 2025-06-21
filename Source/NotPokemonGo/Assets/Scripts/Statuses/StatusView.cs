using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Statuses
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _text;

        private Status _status;

        public StatusType StatusType => _status.Setup.Type;
        public bool HasStatus => _status != null;

        public void Initialize(Status status, Sprite icon = null)
        {
            _status = status;
            
            if (icon != null)
                _icon.sprite = icon;

            _icon.gameObject.SetActive(true);
        }

        public void Dispose() => 
            _status = null;
        
        private void Update()
        {
            if (_status == null)
            {
                _icon.gameObject.SetActive(false);
                return;
            }

            if (_status.TargetTime <= 0f)
            {
                _icon.fillAmount = 0f;
                _icon.gameObject.SetActive(false);
                return;
            }
            
            _text.text = _status.TickCount.ToString(CultureInfo.InvariantCulture);

            float ratio = Mathf.Clamp01(1f - (_status.СurrentTimer / _status.TargetTime));
            _icon.fillAmount = ratio;

            _icon.gameObject.SetActive(ratio > 0f);
        }
    }
}
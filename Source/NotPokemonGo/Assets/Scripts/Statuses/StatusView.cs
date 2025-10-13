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
            _icon.sprite = icon;
            _text.text = _status.TickCount.ToString(CultureInfo.InvariantCulture);

            transform.gameObject.SetActive(true);
        }

        public void Dispose()
        {
            transform.gameObject.SetActive(false);
        }

        public void Tick()
        {
            if (_status == null)
            {
                return;
            }

            Debug.Log($"_status.TickCount во вью = {_status.TickCount}");
            
            _text.text = _status.TickCount.ToString(CultureInfo.InvariantCulture);
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Characters
{
    public class CharacteristicItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _textValue;
        [SerializeField] private TextMeshProUGUI _textType;

        public void Initialize(Sprite icon, string value, string type)
        {
            _icon.sprite = icon;
            _textValue.text = value;
            _textType.text = type;
        }
    }
}
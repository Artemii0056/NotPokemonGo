using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicItemView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _text;

    public void Initialize(Sprite icon, TextMeshProUGUI text)
    {
        _icon.sprite = icon;
        _text = text;
    }
}
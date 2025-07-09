using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.QTE
{
    public class QTEButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;

        public void Initialize(string text, Sprite sprite)
        {
            _text.text = text;
            _image.sprite = sprite;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(Clicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Clicked);
        }

        private void Clicked()
        {
        }
    }
}
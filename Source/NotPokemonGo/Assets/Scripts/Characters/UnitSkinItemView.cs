using System;
using Characters.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Characters
{
    public class UnitSkinItemView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text _priceText;

        [SerializeField] private Sprite _standardBackground;
        [SerializeField] private Sprite _highlightBackground;

        [SerializeField] private Image _contentImage;
        [SerializeField] private Image _lockImage;

        [SerializeField] private Image _selectionText;

        public UnitItemConfig UnitItemConfig { get; private set; }

        private Image _backgroundImage;

        public bool IsLock { get; private set; }

        public event Action<UnitSkinItemView> OnClicked;

        private void Awake()
        {
            _backgroundImage = _contentImage.GetComponent<Image>();
            Unlock();
        }

        public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke(this);

        public void Lock()
        {
            IsLock = true;
            _lockImage.gameObject.SetActive(IsLock);
            _priceText.gameObject.SetActive(true);
        }

        public void Unlock()
        {
            IsLock = false;
            _lockImage.gameObject.SetActive(IsLock);
            _priceText.gameObject.SetActive(IsLock);
        }

        public void Select() =>
            _selectionText.gameObject.SetActive(true);

        public void Deselect() =>
            _selectionText.gameObject.SetActive(false);

        public void Highlight() =>
            _backgroundImage.sprite = _highlightBackground;

        public void UnHighlight() =>
            _backgroundImage.sprite = _standardBackground;

        public void InitImage(UnitItemConfig config)
        {
            _contentImage.sprite = config.ContentImage;
            UnitItemConfig = config;
        }
    }
}
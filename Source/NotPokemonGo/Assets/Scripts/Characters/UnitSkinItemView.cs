using System;
using Characters.Configs;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Characters
{
    public class UnitSkinItemView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UnitSelectionPanel _selectedPanel;
        [SerializeField] private UnitSelectionPanel _unselectedPanel;

        public UnitItemConfig UnitItemConfig { get; private set; }
        //энам
        //вью

        public bool IsFree { get; private set; } = true;

        public event Action<UnitSkinItemView> OnClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsFree) 
                OnClicked?.Invoke(this);
        }

        public void SetBusy()
        {
            IsFree = false;

            _selectedPanel.gameObject.SetActive(true);
            _unselectedPanel.gameObject.SetActive(IsFree);
        }

        public void SetFree()
        {
            IsFree = true;

            _selectedPanel.gameObject.SetActive(false);
            _unselectedPanel.gameObject.SetActive(IsFree);
        }

        public void InitImage(UnitItemConfig config)
        {
            _selectedPanel.SetIcon(config.ContentImage);
            _unselectedPanel.SetIcon(config.ContentImage);
            UnitItemConfig = config;
        }
    }
}
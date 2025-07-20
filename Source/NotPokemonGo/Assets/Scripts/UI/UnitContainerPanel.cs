using System;
using System.Collections.Generic;
using Characters;
using Characters.Configs;
using UnityEngine;

namespace UI
{
    public class UnitContainerPanel : MonoBehaviour
    {
        [SerializeField] private Transform _gridLayoutGroupTransform;

        private List<UnitSkinItemView> _characterSkinItemViews = new List<UnitSkinItemView>();

        public event Action<UnitSkinItemView> Clicked;

        public void AddItems(List<UnitSkinItemView> characterSkinItemViews)
        {
            foreach (var skin in characterSkinItemViews)
            {
                _characterSkinItemViews.Add(skin);
                skin.transform.SetParent(_gridLayoutGroupTransform, false);
                skin.gameObject.SetActive(false);
            }
        }

        public void Show()
        {
            foreach (var characterSkin in _characterSkinItemViews)
            {
                characterSkin.gameObject.SetActive(true);
                characterSkin.OnClicked += OnSkinClicked;
            }
        }

        public void Hide()
        {
            foreach (var characterSkin in _characterSkinItemViews)
            {
                characterSkin.gameObject.SetActive(false);
                characterSkin.OnClicked -= OnSkinClicked;
            }
        }

        public void Release(UnitType unitType)
        {
            foreach (var itemView in _characterSkinItemViews)
            {
                if (itemView.UnitItemConfig.Type == unitType) 
                    itemView.SetFree();
            }
        }
        
        private void OnSkinClicked(UnitSkinItemView itemView) => 
            Clicked?.Invoke(itemView);
    }
}
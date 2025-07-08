using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace UI
{
    public class UnitContainerPanel : MonoBehaviour
    {
        [SerializeField] private Transform _gridLayoutGroupTransform;

        private List<UnitSkinItemView> _characterSkinItemViews = new List<UnitSkinItemView>();
        
        public event Action<UnitSkinItemView> Clicked;

        public void AddItem(UnitSkinItemView skinItemView)
        {
            _characterSkinItemViews.Add(skinItemView);
            skinItemView.transform.SetParent(_gridLayoutGroupTransform, false);
            skinItemView.gameObject.SetActive(false);
        }

        public void Show()
        {
            foreach (var characterSkin in _characterSkinItemViews)
            {
                characterSkin.gameObject.SetActive(true);
                characterSkin.OnClicked += OnSkinClicked;
            }
        }

        private void OnSkinClicked(UnitSkinItemView itemView) => 
            Clicked?.Invoke(itemView);
    }
}
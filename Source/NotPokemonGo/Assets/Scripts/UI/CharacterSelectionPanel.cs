using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace UI
{
    public class CharacterSelectionPanel : MonoBehaviour
    {
        [SerializeField] private Transform _gridLayoutGroupTransform;

        private List<CharacterSkinItemView> _characterSkinItemViews = new List<CharacterSkinItemView>();
        
        public event Action<CharacterSkinItemView> Clicked;

        public void AddItem(CharacterSkinItemView skinItemView)
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

        private void OnSkinClicked(CharacterSkinItemView itemView) => 
            Clicked?.Invoke(itemView);
    }
}
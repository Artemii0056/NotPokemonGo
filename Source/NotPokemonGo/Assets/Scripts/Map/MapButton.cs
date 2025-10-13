using System;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapButton : MonoBehaviour
    {
        [field: SerializeField] public MapType MapType { get; private set; }

        [SerializeField] private Button _button;
        
        public event Action<MapType> OnClick;
        
        private void OnEnable() => 
            _button.onClick.AddListener(() => OnClick?.Invoke(MapType));

        private void OnDisable() => 
            _button.onClick.RemoveAllListeners();
    }
}
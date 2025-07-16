using System;
using Map;
using UnityEngine;

namespace LevelSetting
{
    public class LevelButton : MonoBehaviour
    {
        [field: SerializeField] public  MapType MapType { get; private set; }
        
        public event Action<MapType> OnClick;

        private void OnMouseUpAsButton() => 
            OnClick?.Invoke(MapType);
    }
}
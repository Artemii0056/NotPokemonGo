using System;
using UnityEngine;

namespace LevelSetting
{
    public class LevelButton : MonoBehaviour
    {
        [field: SerializeField] public  LevelType LevelType { get; private set; }
        
        public event Action<LevelType> OnClick;

        private void OnMouseUpAsButton() => 
            OnClick?.Invoke(LevelType);
    }
}
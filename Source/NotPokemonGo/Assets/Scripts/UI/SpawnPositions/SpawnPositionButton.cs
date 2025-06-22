using System;
using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SpawnPositions
{
    [RequireComponent(typeof(Button))]
    public class SpawnPositionButton : MonoBehaviour
    {
        public SpawnPositionType SpawnPositionType;
        private Button _button;

        public event Action<SpawnPositionType> OnClick;

        private void Awake() => 
            _button = GetComponent<Button>();

        private void OnEnable() => 
            _button.onClick.AddListener(Clicked);

        private void OnDisable() => 
            _button.onClick.RemoveListener(Clicked);

        private void Clicked()
        {
            OnClick?.Invoke(SpawnPositionType);
        }
    }
}
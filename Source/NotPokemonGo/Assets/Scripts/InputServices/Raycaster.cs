using System;
using Units;
using UnityEngine;

namespace InputServices
{
    public class Raycaster : MonoBehaviour
    {
        private InputReader _inputReader;
        
        public event Action<Unit> UnitSearched;

        private void Awake() => 
            _inputReader = GetComponent<InputReader>();

        private void OnEnable() => 
            _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonPressed;

        private void OnDisable() => 
            _inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonPressed;

        private void OnLeftMouseButtonPressed()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Unit unit))
                {
                    UnitSearched?.Invoke(unit);
                }
            }
        }
    }
}
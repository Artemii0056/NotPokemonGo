using System;
using Units;
using UnityEngine;

namespace InputServices
{
    public class Raycaster : IRaycaster
    {
        private IInputReader _inputReader;

        public Raycaster(IInputReader inputReader)
        {
            _inputReader = inputReader;
            _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonPressed; 
        }
        
/// <summary>
/// говно
/// </summary>
        ~Raycaster()
        {
            _inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonPressed; 
        }
        
        public event Action<Unit> UnitSearched;

        public void OnLeftMouseButtonPressed()
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
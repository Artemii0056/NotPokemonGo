using System;
using InputServices;
using Services.InputServices;
using UnityEngine;

namespace Services.RaycastServices
{
    public class RaycastService<T> : IRaycastService<T>  where T : MonoBehaviour
    {
        private IInputReader _inputReader;
        public event Action<T> Raycasted;
        public event Action NotCollided;
        public event Action NotFinded;
        
        public RaycastService(IInputReader inputReader)
        {
            _inputReader = inputReader;
            _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonPressed; 
        }

        private void OnLeftMouseButtonPressed()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out T component))
                    Raycasted?.Invoke(component);
                else
                    NotFinded?.Invoke();
            }
            else
            {
                NotCollided?.Invoke();
            }
        }
    }
}
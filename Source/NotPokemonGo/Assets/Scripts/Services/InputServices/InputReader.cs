using System;
using InputServices;
using UnityEngine;

namespace Services.InputServices
{
    public class InputReader : MonoBehaviour, IInputReader
    {
        public event Action LeftMouseButtonPressed; 
        public event Action SpacePressed;
        public event Action EButtonPressed;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LeftMouseButtonPressed?.Invoke();
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpacePressed?.Invoke();
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                EButtonPressed?.Invoke();
            }
        }
    }
}
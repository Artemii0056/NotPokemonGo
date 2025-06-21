using System;
using Units;
using UnityEngine;

namespace InputServices
{
    public class InputReader : MonoBehaviour, IInputReader
    {
        public event Action LeftMouseButtonPressed; 

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
                LeftMouseButtonPressed?.Invoke();
        }
    }
}
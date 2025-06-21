using System;
using Units;
using UnityEngine;

namespace InputServices
{
    public class InputReader : MonoBehaviour
    {
        private Unit _currentUnit;
        
        public event Action LeftMouseButtonPressed; 

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
                LeftMouseButtonPressed?.Invoke();
        }
    }
}
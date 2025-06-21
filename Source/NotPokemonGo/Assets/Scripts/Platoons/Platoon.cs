using System.Collections;
using System.Collections.Generic;
using Characters;
using Services;
using Units;
using UnityEngine;

namespace DefaultNamespace
{
    public class Platoon
    {
        private readonly List<Unit> _characters;

        public Platoon(List<Unit> characters)
        {
            _characters = characters;
        }

        public bool IsAlive { get; private set; }

        public void Tick(float deltaTime)
        {
            foreach (Unit unit in _characters)
            {
                unit.Tick(deltaTime);
            }
        }
    }
}
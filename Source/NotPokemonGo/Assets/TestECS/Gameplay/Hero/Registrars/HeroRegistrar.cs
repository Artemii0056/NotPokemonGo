using System;
using Code.Entity;
using UnityEngine;

namespace TestECS.Gameplay.Hero.Registrars
{
    public class HeroRegistrar : MonoBehaviour
    {
        public int Speed = 2;
        
        private GameEntity _entity;

        private void Start()
        {
            _entity = CreateEntity.
                Empty()
                .AddId(1)
                .AddSpeed(Speed)
                .AddWorldPosition(transform.position)
                .AddDirection(transform.forward);
        }
    }
}
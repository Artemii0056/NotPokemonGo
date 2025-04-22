using UnityEngine;

namespace TestECS.Gameplay.Hero.Factory
{
    public interface IHeroFactory
    {
        GameEntity Create(Vector3 at);
    }
}
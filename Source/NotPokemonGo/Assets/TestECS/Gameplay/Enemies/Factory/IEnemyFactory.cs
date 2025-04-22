using UnityEngine;

namespace TestECS.Gameplay.Enemies.Factory
{
    public interface IEnemyFactory
    {
        GameEntity Create(EnemyTypeId typeId, Vector3 at);
    }
}
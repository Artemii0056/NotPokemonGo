using UnityEngine;

namespace TestECS.Gameplay.Features.Armaments.Factory
{
    public interface IArmamentsFactory
    {
        GameEntity CreateVegetableBolt(int level, Vector3 at);
        GameEntity CreateRadialBolt(int level, Vector3 at);
    }
}
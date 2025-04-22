using Entitas;

namespace TestECS.Gameplay.Enemies
{
        [Game]public class Enemy : IComponent { }
        [Game]public class EnemyAnimatorComponent : IComponent { public EnemyAnimator Value; }
        [Game] public class EnemyTypeIdComponent : IComponent { public EnemyTypeId Value; }
        [Game] public class SpawnTimer : IComponent { public float Value; }
    }

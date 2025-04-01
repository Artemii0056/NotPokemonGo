using Entitas;
using UnityEngine;

namespace Code.Gameplay.FirstComponent
{
    [Game]public class Health : IComponent { public float Value; }
    [Game] public class Damage : IComponent { public float Value; }
    [Game] public class Speed : IComponent { public float Value; }
    [Game] public class TransformComponent : IComponent { public Transform Value; }
}
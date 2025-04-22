using Entitas;
using UnityEngine;

namespace TestECS.Gameplay.Features.Movement
{
    [Game]public class Speed : IComponent { public float Value; }
    [Game] public class Direction : IComponent { public Vector3 Value; }
    [Game] public class Moving : IComponent {  }
    [Game] public class MovementAvailable : IComponent {  }
}
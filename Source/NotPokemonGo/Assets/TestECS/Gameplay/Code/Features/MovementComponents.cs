using Entitas;
using UnityEngine;

namespace TestECS.Gameplay.Code.Features
{
    [Game]public class Speed : IComponent { public int Value; }
    [Game] public class Direction : IComponent { public Vector2 Value; }
    [Game] public class WorldPosition : IComponent { public Vector3 Value; }
    [Game] public class Moving : IComponent {  }
}
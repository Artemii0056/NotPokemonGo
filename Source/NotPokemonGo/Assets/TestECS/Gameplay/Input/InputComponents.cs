using Entitas;
using UnityEngine;

namespace TestECS.Gameplay.Input
{
    public class InputComponents
    {
        [Game] public class Input : IComponent {}
        [Game] public class AxisInput : IComponent {public Vector2 Value;}
    }
}
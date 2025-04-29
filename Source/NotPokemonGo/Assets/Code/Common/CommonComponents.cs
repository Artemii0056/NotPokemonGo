using Code.Gameplay.Common.Visuals.StatusVisuals;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Infrastructure.View;
using TestECS.Gameplay.Code.Common.DamageTakenAnimator;
using UnityEngine;

namespace Code.Common
{
    public class CommonComponents
    {
        [Game] public class View : IComponent { public IEntityView Value; }
        [Game] public class ViewPath : IComponent { public string Value; }
        [Game] public class ViewPrefab : IComponent { public EntityBehavior Value; }
        [Game] public class Destructed : IComponent { }
        [Game] public class SelfDestructTimer : IComponent {public float Value; }
        [Game] public class Damage : IComponent {public float Value; }
        [Game] public class DamageTakenAnimatorComponent : IComponent {public IDamageTakenAnimator Value; }
        [Game] public class StatusVisualsComponent : IComponent {public IStatusVisuals Value; }
        [Game] public class Id : IComponent { [PrimaryEntityIndex]public int Value; }
        [Game] public class WorldPosition : IComponent { public Vector3 Value; }
        [Game] public class TransformComponent : IComponent { public Transform Value; }
    }
}
using System.Collections.Generic;
using Entitas;

namespace TestECS.Gameplay.TargetCollection
{
    public class TargetCollectionComponents
    {
        [Game] public class TargetsBuffer : IComponent {public List<int> Value;}
        [Game] public class TargetId : IComponent {public int Value;}
        [Game] public class ProcessedTargets : IComponent {public List<int> Value;}
        [Game] public class CollectTargetsInterval : IComponent {public float Value;}
        [Game] public class CollectTargetsTimer : IComponent {public float Value;}
        [Game] public class Radius : IComponent {public float Value;}
        [Game] public class LayerMask : IComponent {public int Value;}
        [Game] public class ReadyToCollectTargets : IComponent {}
        [Game] public class CollectingTargetsContinuously : IComponent {}
    }
}
using System.Collections.Generic;
using Abilities.Configs;
using Armaments.Movers;
using QteSystem;
using QteSystem.TestQte;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Runtime
{
    public sealed class AbilityContext
    {
        public Unit Source { get; internal set; }
        public Unit Target { get; internal set; }
        
        public Vector3 StartPosition { get; internal set; }

        public UnitAnimatorTrigger AnimatorTrigger { get; internal set; }
        public AnimatorController Animator { get; internal set; }

        public AbilityPhase CurrentPhase { get; internal set; }
        
        public QteResult QteResult { get; internal set; }
        public IQteSession ActiveQte { get; set; }

        public readonly List<IArmamentMover> Movers  = new List<IArmamentMover>();
    }
}

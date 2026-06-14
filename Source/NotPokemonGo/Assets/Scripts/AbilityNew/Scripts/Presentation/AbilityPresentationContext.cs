using QteSystem.Core;
using Units;
using UnityEngine;

namespace AbilityNew.Scripts.Presentation
{
    public sealed class AbilityPresentationContext
    {
        public Unit Caster;
        public Unit Target;
        public AbilitySo Ability;
        public AbilityPresentationSignal Signal;
        
        public QteSeriesResult QteSeriesResult;

        public Transform ExplicitTransform;
        
        public QteResult? QteResult;
        public bool IsCriticalHit { get; set; }
        public bool ProjectileReflected { get; set; }
    }
}
using System.Collections.Generic;
using Armaments.Movers;
using QteSystem;
using QteSystem.TestQte;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionState
    {
        public IQteSession ActiveQte { get; set; }
        public QteResult? LastQteResult { get; set; }

        public int? LastDamageResult { get; set; } //DamageResult
        public int? LastHealResult { get; set; } //HealResult

        public Dictionary<string, object> Blackboard { get; } = new();
        public List<object> RuntimeHandles { get; } = new();
        
        public List<IArmamentMover> Movers { get; } = new List<IArmamentMover>();


        public bool IsInterrupted { get; set; }
        public bool IsCancelled { get; set; }
    }
}
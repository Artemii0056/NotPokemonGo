using System.Collections.Generic;
using AbilityNew.Scripts;
using Armaments.Movers;
using QteSystem;
using QteSystem.TestQte;
using UnityEngine;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionState
    {
        public IQteSession ActiveQte { get; set; }
        public QteResult? LastQteResult { get; set; }

        public int? LastDamageResult { get; set; } //DamageResult
        public int? LastHealResult { get; set; } //HealResult

        public AbilityBlackboard AbilityBlackboard { get; set; } = new();

        public List<IArmamentMover> Movers { get; } = new List<IArmamentMover>();
        
        public Queue<Transform> FreeArmamentSpawnPoints { get; } = new();

        public bool IsInterrupted { get; set; }
        public bool IsCancelled { get; set; }
    }
}
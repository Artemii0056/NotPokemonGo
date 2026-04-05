using System.Collections.Generic;
using AbilityNew.Scripts;
using AbilityNew.Scripts.Results;
using Armaments.Movers;
using QteSystem;
using QteSystem.Core;
using QteSystem.Gameplay;
using UnityEngine;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionState
    {
        public IQteSession ActiveQte { get; set; }
        public QteResult? LastQteResult { get; set; }

        public AbilityBlackboard AbilityBlackboard { get; set; } = new();
        public CounterAttackRequest CounterAttackRequest { get; }

        public List<IArmamentMover> Movers { get; } = new List<IArmamentMover>();
        
        public Queue<Transform> FreeArmamentSpawnPoints { get; } = new();

        public bool IsInterrupted { get; set; }
        public bool IsCancelled { get; set; }
    }
}
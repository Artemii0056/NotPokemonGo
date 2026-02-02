using System;
using System.Collections.Generic;
using Armaments;
using Castaments;
using Characters.Configs;
using QteSystem.TestQTE;
using UnityEngine;

namespace Abilities.Outcome
{
    [CreateAssetMenu(fileName = nameof(AbilityOutcomeProfile), menuName = "StaticData/" + nameof(AbilityOutcomeProfile))]
    public class AbilityOutcomeProfile : ScriptableObject
    {
        [field: SerializeField] public UnitType UnitType;
        [field: SerializeField] public AbilityType AbilityType;
        [field: SerializeField] public List<SetupsByQte> Setups;
    }

    [Serializable]
    public class SetupsByQte
    {
        public QteResult QteResult = QteResult.Default;
        public TargetMode TargetMode = TargetMode.Single;
        public ArmamentSetup ArmamentSetup;
        public CastamentSetup CastamentSetup;
    }
}
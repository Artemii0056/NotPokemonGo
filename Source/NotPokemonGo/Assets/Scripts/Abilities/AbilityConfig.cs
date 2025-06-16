using System.Collections.Generic;
using Effects;
using Statuses;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(AbilityConfig), menuName = "StaticData/" + nameof(AbilityConfig))]
    public class AbilityConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        
        [field: SerializeField] public TargetMode TargetMode { get; private set; }
        [field: SerializeField] public float Cost { get; private set; }
        [field: SerializeField] public bool IsArmament { get; private set; }

        [field: SerializeField] public List<StatusSetup> Statuses { get; private set; }
        [field: SerializeField] public List<EffectSetup> EffectInfo { get; private set; }
        
        [field: SerializeField] public AbilityView Prefab { get; private set; }
        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
    }
}
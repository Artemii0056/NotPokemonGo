using Abilities.Configs;
using UnityEngine;

namespace Abilities.StepConfigs
{
    [CreateAssetMenu(fileName = nameof(PlayPhaseStepConfig), menuName = "StaticData/Abilities/Steps/" + nameof(PlayPhaseStepConfig))]
    public sealed class PlayPhaseStepConfig : AbilityStepConfig
    {
        [field: SerializeField] public AbilityPhase Phase { get; private set; }
    }
}

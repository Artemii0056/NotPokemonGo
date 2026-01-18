using Abilities.Configs;
using Units;
using Units.AnimationControllers;

namespace Abilities.Runtime
{
    /// <summary>
    /// Контекст выполнения способности. Содержит текущее состояние проигрывания.
    /// </summary>
    public sealed class AbilityContext
    {
        public Unit Source { get; internal set; }
        public Unit Target { get; internal set; }

        public UnitAnimatorTrigger AnimatorTrigger { get; internal set; }
        public AnimatorController Animator { get; internal set; }

        public AbilityPhase CurrentPhase { get; internal set; }
    }
}

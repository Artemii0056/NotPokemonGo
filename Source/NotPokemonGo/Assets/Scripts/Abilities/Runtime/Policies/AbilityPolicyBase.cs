using Abilities.Configs;
using Abilities.Signals;

namespace Abilities.Runtime.Policies
{
    /// <summary>
    /// Удобная базовая реализация: все методы по умолчанию ничего не делают.
    /// </summary>
    public abstract class AbilityPolicyBase : IAbilityPolicy
    {
        public virtual void OnAbilityStart(AbilityContext ctx) { }
        public virtual void OnAbilityStop(AbilityContext ctx) { }
        public virtual void OnPhaseStart(AbilityContext ctx, AbilityPhase phase) { }
        public virtual void OnSignal(AbilityContext ctx, PhaseSignal signal) { }
        public virtual bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase) => true;
    }
}

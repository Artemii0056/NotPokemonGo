using Abilities.Signals;
using Abilities.Configs;

namespace Abilities.Runtime.Policies
{
    public interface IAbilityPolicy
    {
        bool CanUseAbility(AbilityContext ctx);
        void OnAbilityStart(AbilityContext context);
        void OnAbilityStop(AbilityContext ctx);
        void OnPhaseStart(AbilityContext ctx, AbilityPhase phase);
        void OnSignal(AbilityContext ctx, PhaseSignal signal);

        bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase);
    }
}

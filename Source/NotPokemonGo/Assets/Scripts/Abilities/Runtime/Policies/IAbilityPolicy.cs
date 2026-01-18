using Abilities.Signals;
using Abilities.Configs;

namespace Abilities.Runtime.Policies
{
    /// <summary>
    /// Политика поведения AbilityHandler. Все расширения геймплея подключаются композицией.
    /// </summary>
    public interface IAbilityPolicy
    {
        void OnAbilityStart(AbilityContext ctx);
        void OnAbilityStop(AbilityContext ctx);
        void OnPhaseStart(AbilityContext ctx, AbilityPhase phase);
        void OnSignal(AbilityContext ctx, PhaseSignal signal);

        /// <summary>
        /// Должна вернуть true, если (с точки зрения политики) фазу можно завершить.
        /// Общий критерий завершения фазы — AND по всем политикам.
        /// </summary>
        bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase);
    }
}

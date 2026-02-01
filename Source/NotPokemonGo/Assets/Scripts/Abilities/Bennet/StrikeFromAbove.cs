using System;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Runtime;
using Abilities.Runtime.Policies;
using Services;
using Units;

namespace Abilities.Bennet
{
    /// <summary>
    /// Удар сверху. Реализован через стандартный фазовый проигрыватель.
    /// (Если потребуется особая логика — переопредели ExecutePhase.)
    /// </summary>
    public sealed class StrikeFromAbove : IAbilityHandler
    {
        private readonly ComposedPhasedAbilityHandler _impl;

        public StrikeFromAbove(ICoroutineRunner currentRoutine, AbilityModel abilityModel)
        {
            CurrentAbility =  abilityModel;
            
            _impl = new ComposedPhasedAbilityHandler(
                CurrentAbility,
                currentRoutine,
                new IAbilityPolicy[]
                {
                    new FinishSignalPolicy()
                });
        }

        public AbilityModel CurrentAbility { get; }

        public event Action<IAbilityHandler> Finished
        {
            add => _impl.Finished += value;
            remove => _impl.Finished -= value;
        }

        public void Play(Unit source, Unit target) => _impl.Play(source, target);
        public void Stop() => _impl.Stop();
    }
}
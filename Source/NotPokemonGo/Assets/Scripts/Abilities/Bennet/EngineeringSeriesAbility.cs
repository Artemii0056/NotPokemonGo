using System;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Runtime;
using Abilities.Runtime.Policies;
using QteSystem;
using Services;
using Units;

namespace Abilities.Bennet
{
    /// <summary>
    /// Способность с QTE. Реализована композицией:
    /// - FinishSignalPolicy (таймлайн = анимация)
    /// - QtePhasePolicy     (Rule A: фаза не завершится, пока не завершится QTE)
    /// </summary>
    public sealed class EngineeringSeriesAbility : IAbilityHandler
    {
        private readonly ComposedPhasedAbilityHandler _impl;

        public EngineeringSeriesAbility(AbilityModel abilityModel, ICoroutineRunner coroutineRunner, IQteService qteService)
        {
            CurrentAbility = abilityModel;
            
            _impl = new ComposedPhasedAbilityHandler(
                CurrentAbility,
                coroutineRunner,
                new IAbilityPolicy[]
                {
                    new FinishSignalPolicy(),
                    new QtePhasePolicy(qteService)
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
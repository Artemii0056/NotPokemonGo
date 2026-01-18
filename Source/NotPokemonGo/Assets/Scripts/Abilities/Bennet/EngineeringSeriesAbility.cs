using System;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Runtime;
using Abilities.Runtime.Policies;
using QTESystem;
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
            _impl = new ComposedPhasedAbilityHandler(
                abilityModel,
                coroutineRunner,
                new IAbilityPolicy[]
                {
                    new FinishSignalPolicy(),
                    new QtePhasePolicy(qteService)
                });
        }

        public event Action<IAbilityHandler> Finished
        {
            add => _impl.Finished += value;
            remove => _impl.Finished -= value;
        }

        public void Play(Unit source, Unit target) => _impl.Play(source, target);
        public void Stop() => _impl.Stop();
        public Interruptibility Interruptibility => _impl.Interruptibility;
    }
}
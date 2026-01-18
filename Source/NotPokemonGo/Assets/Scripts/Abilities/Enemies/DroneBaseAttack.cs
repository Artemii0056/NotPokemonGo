using System;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Runtime;
using Abilities.Runtime.Policies;
using Services;
using Units;

namespace Abilities.Enemies
{
    /// <summary>
    /// Атака дрона — тот же фазовый проигрыватель.
    /// </summary>
    public sealed class DroneBaseAttack : IAbilityHandler
    {
        private readonly ComposedPhasedAbilityHandler _impl;

        public DroneBaseAttack(AbilityModel abilityModel, ICoroutineRunner coroutineRunner)
        {
            _impl = new ComposedPhasedAbilityHandler(
                abilityModel,
                coroutineRunner,
                new IAbilityPolicy[] { new FinishSignalPolicy() });
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
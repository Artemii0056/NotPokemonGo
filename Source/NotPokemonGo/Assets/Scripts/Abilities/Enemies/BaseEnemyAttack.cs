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
    public sealed class BaseEnemyAttack : IAbilityHandler
    {
        private readonly ComposedPhasedAbilityHandler _impl;

        public BaseEnemyAttack(ICoroutineRunner runner, AbilityModel model)
        {
            CurrentAbility = model;

            _impl = new ComposedPhasedAbilityHandler(
                CurrentAbility,
                runner,
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

        public void Play(Unit source, Unit target) => 
            _impl.Play(source, target);
        
        public void Stop() => 
            _impl.Stop();
    }
}
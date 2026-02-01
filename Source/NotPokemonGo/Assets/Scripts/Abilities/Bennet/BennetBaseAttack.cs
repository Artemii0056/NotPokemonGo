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
    /// Обычная ближняя атака. Логика фаз и эффектов — в AbilityPhaseService.
    /// Этот handler лишь проигрывает фазы и ждёт Finish.
    /// </summary>
    public sealed class BennetBaseAttack : IAbilityHandler
    {
        private readonly ComposedPhasedAbilityHandler _impl;


        public BennetBaseAttack(AbilityModel model, ICoroutineRunner runner)
        {
            _impl = new ComposedPhasedAbilityHandler(
                model,
                runner,
                new IAbilityPolicy[]
                {
                    new RememberStartPositionPolicy(),
                    new FinishSignalPolicy()
                });
        }
        
        public AbilityModel CurrentAbility { get; }

        public Interruptibility Interruptibility => _impl.CurrentAbility.Interruptibility;

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
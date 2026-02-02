using System;
using Armaments;
using Armaments.Spawner;
using QteSystem.TestQte;
using Units;
using Object = UnityEngine.Object;

namespace Abilities.Runtime.Impact
{
    public sealed class DefaultShotImpactResolver : IShotImpactResolver
    {
        private readonly IEffectsApplier _effectsApplier;
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly ImpactPolicy _policy;
        private readonly Action<Shot> _startShot; 

        public DefaultShotImpactResolver(
            IEffectsApplier effectsApplier,
            IArmamentSpawner armamentSpawner,
            ImpactPolicy policy,
            Action<Shot> startShot)
        {
            _effectsApplier = effectsApplier;
            _armamentSpawner = armamentSpawner;
            _policy = policy ?? new ImpactPolicy();
            _startShot = startShot;
        }

        public void Resolve(Shot shot)
        {
            if (shot == null) 
                return;

            ImpactAction action = ChooseAction(shot);

            switch (action)
            {
                case ImpactAction.ApplyEffectsAndDestroy:
                    ApplyEffects(shot);
                    DestroyArmament(shot);
                    break;

                case ImpactAction.DestroyOnly:
                    DestroyArmament(shot);
                    break;

                case ImpactAction.ReflectToSourceAndDestroy:
                    Reflect(shot);
                    DestroyArmament(shot);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ImpactAction ChooseAction(Shot shot)
        {
            if (!shot.RequiresQte)
                return _policy.NoQteAction;

            QteResult result = shot.QteResult ?? _policy.DefaultIfMissing;

            return result switch
            {
                QteResult.Fail => _policy.OnFail,
                QteResult.Normal => _policy.OnNormal,
                QteResult.Perfect => _policy.OnPerfect,
                _ => _policy.OnFail
            };
        }

        private void ApplyEffects(Shot shot)
        {
            Armament armament = shot.Mover?.Armament;
            
            if (armament == null) 
                return;

            _effectsApplier.ApplyEffectsOnTarget(
                shot.Context.Source,
                shot.Context.Target,
                armament.Statuses,
                armament.Effects);
        }

        private void Reflect(Shot original)
        {
            Unit newSource = original.Context.Target;
            Unit newTarget = original.Context.Source;

            ArmamentSetup setup = original.Context.Setup;

            ArmamentContext context = new ArmamentContext(newSource, newTarget, setup, ArmamentFlyingType.Direct);
            IArmamentMover mover = _armamentSpawner.Create(context);

            Shot reflected = new Shot(original.Phase, context, mover)
            {
                RequiresQte = false
            };

            _startShot?.Invoke(reflected);
        }

        private void DestroyArmament(Shot shot)
        {
            Armament armament = shot.Mover?.Armament;
            
            if (armament != null)
                Object.Destroy(armament.gameObject);
        }
    }
}

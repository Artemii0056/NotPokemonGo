using System;
using Armaments;
using Armaments.Movers;
using Effects;
using QteSystem.TestQTE;
using Spawners.Spawner;
using Statuses;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abilities.Runtime.Impact
{
    public sealed class DefaultShotImpactResolver : IShotImpactResolver //TODO Изменить 
    {
        private readonly IEffectsApplier _effectsApplier;
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly ImpactPolicy _policy;
        private readonly Action<Shot> _startShot;

        private int _currentCount;

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

            _currentCount = 0;
        }

        public void Resolve2(Shot shot)
        {
            if (shot == null)
                return;

            Unit target = shot.Context.Target; 
            
            if (target.HasStatus(StatusType.Bubble))
            {
                Debug.Log("HaveStatus");
                
                _currentCount++;
                
                if (_currentCount >= 3)
                {
                    Reflect(shot);
                }
            }
            else
            {
                ApplyEffects(shot);
            }

            DestroyArmament(shot);
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
                shot.Context.Target,
                armament.Statuses,
                armament.Effects);
        }

        private void Reflect(Shot original)
        {
            ArmamentContext originalContext = original.Context;
            
            Unit newSource = originalContext.Target;
            Unit newTarget = originalContext.Source;

            ArmamentSetup setup = originalContext.Setup; //Тут нужно получить армамент с другими параметрами 

            ArmamentContext context = new ArmamentContext(newSource, newTarget, setup, ArmamentFlyingType.Direct,
                newSource.abilityPos, originalContext); 
            
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
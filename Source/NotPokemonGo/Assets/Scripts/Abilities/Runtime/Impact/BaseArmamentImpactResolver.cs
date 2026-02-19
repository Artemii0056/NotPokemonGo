using System;
using Armaments;
using Armaments.Movers;
using Effects;
using Spawners.Spawner;
using Statuses;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abilities.Runtime.Impact
{
    public class BaseArmamentImpactResolver
    {
        private readonly IEffectsApplier _effectsApplier;
        private readonly IArmamentSpawner _armamentSpawner;

        private int _currentCount;

        public BaseArmamentImpactResolver(
            IEffectsApplier effectsApplier,
            IArmamentSpawner armamentSpawner,
            Action<IArmamentMover> startShot)
        {
            _effectsApplier = effectsApplier;
            _armamentSpawner = armamentSpawner;
            _startShot = startShot;

            _currentCount = 0;
        }
        
        private readonly Action<IArmamentMover> _startShot;

        public void Resolve(IArmamentMover mover)
        {
            Debug.Log("Resolving...");
            
            if (mover == null)
                return;
            
            Debug.Log("Off");

            Unit target = mover.Armament.Target;

            if (target.HaveStatus(StatusType.Bubble))
            {
                Debug.Log("HaveStatus");

                _currentCount++;

                if (_currentCount >= 3) //передать 
                {
                    Reflect(mover.Armament);
                }
            }
            else
            {
                ApplyEffects(mover);
            }

            DestroyArmament(mover);
        }

        private void ApplyEffects(IArmamentMover mover)
        {
            Armament armament = mover.Armament;

            if (armament == null)
                return;

            _effectsApplier.ApplyEffectsOnTarget(
                mover.Armament.Context.Target,
                armament.Statuses,
                armament.Effects);
        }

        private void Reflect(Armament armament)
        {
            Unit newSource = armament.Target;
            Unit newTarget = armament.Source;

            ArmamentSetup setup = armament.Setup; 

            ArmamentContext context = new ArmamentContext(newSource, newTarget, setup, ArmamentFlyingType.Direct,
                newSource.abilityPos, armament.Context);

            IArmamentMover mover = _armamentSpawner.Create(context);

            _startShot?.Invoke(mover);
        }

        private void DestroyArmament(IArmamentMover mover)
        {
            Armament armament = mover?.Armament;

            if (armament != null)
                Object.Destroy(armament.gameObject);
        }
    }
}
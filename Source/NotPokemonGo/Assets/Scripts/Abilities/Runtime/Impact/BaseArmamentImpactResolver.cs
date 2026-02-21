using System;
using Armaments;
using Armaments.Movers;
using Effects;
using Spawners.Spawner;
using Object = UnityEngine.Object;

namespace Abilities.Runtime.Impact
{
    public class BaseArmamentImpactResolver
    {
        private readonly IEffectsApplier _effectsApplier;
        private readonly IArmamentSpawner _armamentSpawner;

        public BaseArmamentImpactResolver(IEffectsApplier effectsApplier) =>
            _effectsApplier = effectsApplier;

        private readonly Action<IArmamentMover> _startShot;

        public void Resolve(IArmamentMover mover)
        {
            if (mover == null)
                return;

            ApplyEffects(mover);
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

        private void DestroyArmament(IArmamentMover mover)
        {
            Armament armament = mover?.Armament;

            if (armament != null)
                Object.Destroy(armament.gameObject);
        }
    }
}
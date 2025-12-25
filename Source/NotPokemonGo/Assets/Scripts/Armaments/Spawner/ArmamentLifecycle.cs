using ReactionSystems;
using UnityEngine;

namespace Armaments.Spawner
{
    public class ArmamentLifecycle : IArmamentLifecycle
    {
        private readonly IEffectsApplier _effectsApplier;
        private readonly IReactionService _reactionService;

        public ArmamentLifecycle(
            IReactionService reactionService,
            IEffectsApplier effectsApplier)
        {
            _reactionService = reactionService;
            _effectsApplier = effectsApplier;
        }

        public void Register(IArmamentMover armamentMover) =>
            armamentMover.Reached += OnReached;

        private void OnReached(IArmamentMover mover)
        {
            Armament armament = mover.Armament;
            mover.Reached -= OnReached;

            var context = new ReactionContext(
                armament.Source,
                armament.Target,
                armament);

            Object.Destroy(armament.gameObject);

            if (_reactionService.TryReact(context))
                return;

            _effectsApplier.ApplyEffectsOnTarget(armament.Source, armament.Target, armament.Statuses, armament.Effects);
        }
    }
}
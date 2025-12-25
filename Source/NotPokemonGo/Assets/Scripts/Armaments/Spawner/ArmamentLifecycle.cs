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

        public IArmamentMover Register(Armament armament)
        {
            Debug.Log($"Registering armament {armament.name}");
            
            IArmamentMover armamentMover = new ArmamentMover();
            armamentMover.Reached += OnReached;
            //armamentMover.Move(armament);

            return armamentMover;
        }

        private void OnReached(IArmamentMover mover)
        {
            Debug.Log("Lifecycle OnReached START");

            if (mover.Armament == null)
            {
                Debug.LogError("Armament is NULL in Lifecycle.OnReached");
                return;
            }

            Debug.Log("Lifecycle OnReached HAS ARMAMENT");

            Armament armament = mover.Armament;
            mover.Reached -= OnReached;

            var context = new ReactionContext(
                armament.Source,
                armament.Target,
                armament);

            //Object.Destroy(armament.gameObject); //ВОТ ТУТ

            if (_reactionService.TryReact(context))
                return;

            _effectsApplier.ApplyEffectsOnTarget(armament.Source, armament.Target, armament.Statuses, armament.Effects);
        }
    }
}
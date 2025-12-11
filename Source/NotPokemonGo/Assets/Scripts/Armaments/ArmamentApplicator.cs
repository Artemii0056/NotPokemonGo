using System.Collections.Generic;
using System.Linq;
using Effects;
using ReactionSystems;
using Statuses;
using Statuses.Services;
using Units;
using UnityEngine;

namespace Armaments
{
    public class ArmamentApplicator : IArmamentApplicator
    {
        private readonly IArmamentViewFactory _armamentViewFactory;
        private readonly IStatusFactory _statusFactory;
        private readonly IEffectResolver _effectResolver;
        private readonly IStatusResolver _statusResolver;
        private readonly IReactionService _reactionService;

        public ArmamentApplicator(
            IArmamentViewFactory armamentViewFactory,
            IStatusFactory statusFactory,
            IEffectResolver effectResolver,
            IStatusResolver statusResolver,
            IReactionService reactionService)
        {
            _armamentViewFactory = armamentViewFactory;
            _statusFactory = statusFactory;
            _effectResolver = effectResolver;
            _statusResolver = statusResolver;
            _reactionService = reactionService;
        }

        public void Apply(ArmamentSetup setup, ArmamentFlyingType flyingType, Unit source, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = CreateEffects(setup.EffectsSetup);
                List<Status> statuses = CreateStatuses(setup.Statuses, source, target);
                
                Armament armament =
                    _armamentViewFactory.Create(
                        effects,
                        statuses, //TODO Есть сетап - это и 
                        source.abilityPos.position, //TODO Связать с абилити энкором
                        setup.ArmamentPrefab,
                        source,
                        target,
                        setup); 

                CreateMover(armament, flyingType);
            }
        }

        private void CreateMover(Armament armament, ArmamentFlyingType flyingType)
        {
            IArmamentMover mover = new ArmamentMover();
            mover.Move(armament, flyingType);
            mover.Reached += OnReached;
        }

        private void OnReached(Armament armament, IArmamentMover mover)
        {
            mover.Reached -= OnReached;

            var context = new ReactionContext(
                source: armament.Source,
                target: armament.Target,
                armament);
            
            Object.Destroy(armament);

            if (_reactionService.TryReact(context))
                return;
            
            ApplyEffectsOnTarget(armament.Source, armament.Target, armament.Statuses, armament.Effects);

            // if (_dodgeService.CanDodge(armament.Target)) 
            //     Apply(_dodgeService.Dodge(armament), armament.FlyingType);
            // else
            //     ApplyEffectsOnTarget(armament.Source, armament.Target, armament.Statuses, armament.Effects);
        }

        private List<EffectInfo> CreateEffects(List<EffectSetup> effects) =>
            effects.Select(s => new EffectInfo(s.Value, s.TargetType, s.Type, s.DamageType)).ToList();

        private List<Status> CreateStatuses(IEnumerable<StatusSetup> setups, Unit source, Unit target) =>
            setups.Select(s => _statusFactory.Create(s, source, target, _effectResolver)).ToList();

        private void ApplyEffectsOnTarget(Unit source, Unit target, List<Status> statuses, List<EffectInfo> effects)
        {
            foreach (var status in statuses)
                _statusResolver.Resolve(status, target);

            foreach (var effectInfo in effects)
                _effectResolver.ApplyEffect(source, target, effectInfo);
        }
    }
}
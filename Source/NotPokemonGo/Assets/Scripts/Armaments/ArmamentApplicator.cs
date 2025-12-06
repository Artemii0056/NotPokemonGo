using System.Collections.Generic;
using System.Linq;
using DodgeSystem;
using Effects;
using Services;
using Statuses;
using Statuses.Services;
using Units;
using UnityEngine;

namespace Armaments
{
    public class ArmamentApplicator : IArmamentApplicator
    {
        private readonly IArmamentViewFactory _armamentViewFactory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IStatusFactory _statusFactory;
        private readonly IEffectResolver _effectResolver;
        private readonly IStatusResolver _statusResolver;
        private readonly IDodgeService _dodgeService;

        public ArmamentApplicator(
            IArmamentViewFactory armamentViewFactory,
            IStatusFactory statusFactory,
            IEffectResolver effectResolver,
            IStatusResolver statusResolver,
            IDodgeService dodgeService,
            ICoroutineRunner coroutineRunner)
        {
            _armamentViewFactory = armamentViewFactory;
            _statusFactory = statusFactory;
            _effectResolver = effectResolver;
            _statusResolver = statusResolver;
            _dodgeService = dodgeService;
            _coroutineRunner = coroutineRunner;
        }

        public void Apply(ArmamentSetup setup, Unit source, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = CreateEffects(setup.EffectsSetup);
                List<Status> statuses = CreateStatuses(setup.Statuses, source, target);

                Armament armament =
                    _armamentViewFactory.Create(
                        effects,
                        statuses,
                        source.abilityPos.position, //TODO Связать с абилити энкором
                        setup.ArmamentPrefab,
                        source,
                        target); //Сюда трансформ? 

                CreateMover(armament);
            }
        }

        public void Apply(Armament armament)
        {
            CreateMover(armament, true);
        }

        private void CreateMover(Armament armament, bool isReturn = false)
        {
            ArmamentMover mover = new ArmamentMover(_coroutineRunner);
            mover.Move(armament, isReturn);
            mover.Reached += OnReached;
        }

        private void OnReached(Armament armament, ArmamentMover mover)
        {
            mover.Reached -= OnReached;
            Object.Destroy(armament);

            if (_dodgeService.CanDodge(armament.Target))
                Apply(_dodgeService.Dodge(armament));
            else
                ApplyEffectsOnTarget(armament.Source, armament.Target, armament.Statuses, armament.Effects);
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
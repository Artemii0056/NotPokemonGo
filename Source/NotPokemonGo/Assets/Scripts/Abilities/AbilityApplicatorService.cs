using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.MV;
using Effects;
using Factories;
using Services;
using Statuses;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityApplicatorService
    {
        private AbilityModel _abilityModel;

        private ICoroutineRunner _coroutineRunner;

        private Unit _source;
        private ArmamentViewFactory _armamentViewFactory;
        private StatusFactory _statusFactory;
        private EffectResolver _effectResolver;
        private EffectManager _effectManager;

        public AbilityApplicatorService(ArmamentViewFactory armamentViewFactory,
            StatusFactory statusFactory, EffectResolver effectResolver, EffectManager effectManager,
            ICoroutineRunner coroutineRunner)
        {
            _armamentViewFactory = armamentViewFactory;
            _statusFactory = statusFactory;
            _effectResolver = effectResolver;
            _effectManager = effectManager;
            _coroutineRunner = coroutineRunner;
        }

        public void Remember(AbilityModel abilityModel) =>
            _abilityModel = abilityModel;

        public void RememberSource(Unit unit) =>
            _source = unit;

        public void Apply(params Unit[] targets)
        {
            AbilityModel abilityModel = new AbilityModel(_abilityModel);

            if (abilityModel.HasArmament)
            {
                ApplyArmament(abilityModel, targets);
            }
            else if (abilityModel.HasCastament)
            {
                ApplyCastament(abilityModel, targets);
            }

            _abilityModel.DiscardCurrentTime();
        }

        private void ApplyCastament(AbilityModel abilityModel, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = CreateEffects(abilityModel.CastamentSetup.EffectsSetup);
                List<Status> statuses = CreateStatuses(abilityModel.CastamentSetup.Statuses, target);

                ApplyEffectsOnTarget(target, statuses, effects);

                ParticleSystem effect = Object.Instantiate(abilityModel.CastamentSetup.ParticleSystem);
                effect.transform.position = target.transform.position;
                effect.Play();
            }
        }

        private void ApplyArmament(AbilityModel abilityModel, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = CreateEffects(abilityModel.ArmamentSetup.EffectsSetup);
                List<Status> statuses = CreateStatuses(abilityModel.ArmamentSetup.Statuses, target);

                ArmamentView armamentView =
                    _armamentViewFactory.Create(_source.abilityPos.position,
                        abilityModel.ArmamentSetup.ArmamentView, target);

                _coroutineRunner.StartCoroutine(PlayArmamentAbility(statuses, effects, armamentView, target));
            }
        }

        private IEnumerator PlayArmamentAbility(List<Status> statuses, List<EffectInfo> effects,
            ArmamentView armamentView, Unit target)
        {
            while (Vector3.Distance(target.transform.position, armamentView.transform.position) > 0.1f)
                yield return null;

            ApplyEffectsOnTarget(target, statuses, effects);
        }

        private List<EffectInfo> CreateEffects(List<EffectSetup> effects) =>
            effects.Select(s => new EffectInfo(s.Type, s.Value)).ToList();

        private List<Status> CreateStatuses(IEnumerable<StatusSetup> setups, Unit target) =>
            setups.Select(s => _statusFactory.Create(s, target, _effectResolver)).ToList();

        private void ApplyEffectsOnTarget(Unit target, List<Status> statuses, List<EffectInfo> effects)
        {
            foreach (var status in statuses)
                _effectManager.RegisterStatusEffect(status);

            foreach (var effectInfo in effects)
                target.ReceiveDamage(effectInfo);
        }
    }
}
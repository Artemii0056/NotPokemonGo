using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;
using Abilities.MV;
using Effects;
using Factories;
using Services;
using Statuses;
using Statuses.Services;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityApplicatorService : IAbilityApplicatorService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IArmamentViewFactory _armamentViewFactory;
        private readonly IStatusFactory _statusFactory;
        private readonly IEffectResolver _effectResolver;
        private readonly IStatusResolver _statusResolver;
        private readonly ISourceProvider _sourceProvider;
        private readonly IAbilityProvider _abilityProvider;

        public AbilityApplicatorService(
            IArmamentViewFactory armamentViewFactory,
            IStatusFactory statusFactory, 
            IEffectResolver effectResolver,
            ICoroutineRunner coroutineRunner, 
            IStatusResolver statusResolver, 
            ISourceProvider sourceProvider,
            IAbilityProvider abilityProvider)
        {
            _armamentViewFactory = armamentViewFactory;
            _statusFactory = statusFactory;
            _effectResolver = effectResolver;
            _coroutineRunner = coroutineRunner;
            _statusResolver = statusResolver;
            _sourceProvider = sourceProvider;
            _abilityProvider = abilityProvider;
        }

        public void Apply(params Unit[] targets)
        {
            AbilityModel abilityModel = _abilityProvider.AbilityModel; // TODO Текущие настройки сюда получить??

            if (abilityModel == null)
                throw new NullReferenceException("AbilityModel is null or not ready");

            if (abilityModel.IsReady() == false)
                throw new NullReferenceException("AbilityModel is not ready");
            
            if (abilityModel.HasArmament)
            {
                ApplyArmament(abilityModel, targets);
            }
            else if (abilityModel.HasCastament)
            {
                ApplyCastament(abilityModel, targets);
            }

            abilityModel.DiscardCurrentTime();
            // _abilityProvider.Discard();
            // _sourceProvider.Discard();
            //TODO Дискарднуть абидити панел
        }

        public void Apply(CastamentSetup setup, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = CreateEffects(setup.EffectsSetup);
                List<Status> statuses = CreateStatuses(setup.Statuses, target);

                ApplyEffectsOnTarget(target, statuses, effects);

                // ParticleSystem effect = Object.Instantiate(abilityModel.CastamentSetup.ParticleSystem);
                // effect.transform.position = target.transform.position;
                // effect.Play();
            }
        }
        
        public void Apply(ArmamentSetup setup, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = CreateEffects(setup.EffectsSetup);
                List<Status> statuses = CreateStatuses(setup.Statuses, target);

                ArmamentView armamentView =
                    _armamentViewFactory.Create(_sourceProvider.Source.abilityPos.position,
                        setup.ArmamentView, target);

                _coroutineRunner.StartCoroutine(PlayArmamentAbility(statuses, effects, armamentView, target));
            }
        }

        private void ApplyCastament(AbilityModel abilityModel, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = CreateEffects(abilityModel.CastamentSetup.EffectsSetup);
                List<Status> statuses = CreateStatuses(abilityModel.CastamentSetup.Statuses, target);

                ApplyEffectsOnTarget(target, statuses, effects);

                // ParticleSystem effect = Object.Instantiate(abilityModel.CastamentSetup.ParticleSystem);
                // effect.transform.position = target.transform.position;
                // effect.Play();
            }
        }

        private void ApplyArmament(AbilityModel abilityModel, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = CreateEffects(abilityModel.ArmamentSetup.EffectsSetup);
                List<Status> statuses = CreateStatuses(abilityModel.ArmamentSetup.Statuses, target);

                ArmamentView armamentView =
                    _armamentViewFactory.Create(_sourceProvider.Source.abilityPos.position,
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
                _statusResolver.Resolve(status, target);

            foreach (var effectInfo in effects)
            {
                target.ReceiveDamage(effectInfo);
            }
        }
    }
}
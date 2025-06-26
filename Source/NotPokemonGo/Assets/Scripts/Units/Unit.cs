using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Abilities.MV;
using Animations;
using Characters;
using Characters.Configs;
using Effects;
using Infrastructure;
using Services.StaticDataServices;
using Stats;
using Statuses;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        public AbilityAnimationControllerBase AbilityAnimationControllerBase;
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private List<Status> _imposedStatuses = new List<Status>();
        private IEffectResolver _effectResolver;
        private Animator _animator;
        

        private List<AbilityModel> _abilityModels = new List<AbilityModel>();
        public event Action<Status> StatusAdded;
        public event Action<Status> StatusRemoved;

        public PlatoonType PlatoonType { get; private set; }
        public UnitStep Step { get; private set; }
        [field: SerializeField] public UnitType UnitType { get; private set; }
        
        public List<Status> ImposedStatuses => _imposedStatuses.ToList();
        public List<AbilityModel> AbilityModels => _abilityModels.ToList();

        public Transform abilityPos;

        public ParticleSystem ImposedEffect;
        public ParticleSystem ExplosionEffect;
        private IParticleSystemFactory _particleSystemFactory;
        private IAbilityProvider _abilityProvider;
        private IStaticDataService _staticDataService;

        public void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void Construct(
            List<StatConfig> statConfig,
            IEffectResolver effectResolver,
            PlatoonType platoonType,
            IParticleSystemFactory particleSystemFactory,
            IAbilityProvider abilityProvider,
            IStaticDataService staticDataService
        )
        {
            _staticDataService = staticDataService;
            _abilityProvider = abilityProvider;
            _particleSystemFactory = particleSystemFactory;
            _effectResolver = effectResolver;
            PlatoonType = platoonType;

            Step = new UnitStep(5);

            foreach (var statSetup in statConfig)
            {
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));
            }
        }

        private void OnEnable()
        {
            AbilityAnimationControllerBase.ParticleSystem1Started += OnParticleSystem1Started;
            AbilityAnimationControllerBase.ParticleSystem2Started += OnParticleSystem2Started;
            AbilityAnimationControllerBase.ParticleSystem3Started += OnParticleSystem3Started;
        }

        private void OnDisable()
        {
            AbilityAnimationControllerBase.ParticleSystem1Started -= OnParticleSystem1Started;
            AbilityAnimationControllerBase.ParticleSystem2Started -= OnParticleSystem2Started;
            AbilityAnimationControllerBase.ParticleSystem3Started -= OnParticleSystem3Started;
        }

        public float GetStat(StatType statType)
        {
            return _stats[statType].CurrentValue;
        }

        public void ReceiveDamage(EffectInfo effectInfo)
        {
            float damage = _effectResolver.CalculateFinalValue(this, effectInfo);
            ChangeValue(StatType.Health, damage);
        }

        public void ChangeValue(StatType statType, float value) => 
            _stats[statType].Modify(value);

        public void AddStatus(Status status)
        {
            StatusAdded?.Invoke(status);
            _imposedStatuses.Add(status);
        }

        public void RemoveStatus(Status status)
        {
            _imposedStatuses.Remove(status);
            StatusRemoved?.Invoke(status);
        }

        public void AddAbility(AbilityModel ability) =>
            _abilityModels.Add(ability);

        public void Tick(float deltaTime)
        {
            if (_abilityModels.Count > 0)
            {
                foreach (var model in _abilityModels)
                    model.UpdateTime(deltaTime);
            }

            Step.IncreaseCurrentValue(deltaTime * GetStat(StatType.Agility));
        }

        private IEnumerator Timer(float time)
        {
            float startTime = 0;

            while (startTime < time)
            {
                startTime += Time.deltaTime;
                
                yield return null;
            }

            //Instantiate(ImposedEffect, transform.position, Quaternion.identity).Play();
        }
        
        private IEnumerator Timer2(float time)
        {
            float startTime = 0;

            while (startTime < time)
            {
                startTime += Time.deltaTime;
                
                yield return null;
            }
            
            //ExplosionEffect.Play();
        }
        
        public float GetDeathAnimationLength(string animationName)
        {
            AnimationClip clip = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == animationName);

            if (clip == null)
                throw new Exception($"Animator not contains animation {animationName}");

            return clip.length;
        }
        
        private void OnParticleSystem3Started()
        {
            AbilityConfig abilityConfig = _staticDataService.GetAbilityConfig(_abilityProvider.AbilityModel.AbilityType);
            _particleSystemFactory.Create(abilityConfig.EndAnimationParticles, transform.position, Quaternion.identity);
        }

        private void OnParticleSystem2Started()
        {
            Debug.Log(_abilityProvider.AbilityModel == null);
            AbilityConfig abilityConfig = _staticDataService.GetAbilityConfig(_abilityProvider.AbilityModel.AbilityType);
            _particleSystemFactory.Create(abilityConfig.MiddleAnimationParticles, transform.position, Quaternion.identity);
        }

        private void OnParticleSystem1Started()
        {
            AbilityConfig abilityConfig = _staticDataService.GetAbilityConfig(_abilityProvider.AbilityModel.AbilityType);
            _particleSystemFactory.Create(abilityConfig.StartAnimationParticles, transform.position, Quaternion.identity);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.MV;
using Characters;
using Effects;
using Infrastructure;
using Stats;
using Statuses;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private List<Status> _imposedStatuses = new List<Status>();
        private IEffectResolver _effectResolver;
        private Animator _animator;
        private AnimatorController _animatorController;

        public event Action<Status> StatusAdded;
        public event Action<Status> StatusRemoved;

        private List<AbilityModel> _abilityModels = new List<AbilityModel>();

        public PlatoonType PlatoonType { get; private set; }
        public UnitStep Step { get; private set; }
        public List<Status> ImposedStatuses => _imposedStatuses.ToList();
        public List<AbilityModel> AbilityModels => _abilityModels.ToList();

        public Transform abilityPos;

        public ParticleSystem ImposedEffect;

        public void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _animatorController = new AnimatorController(_animator);
        }

        public void Initialize(
            List<StatConfig> statConfig,
            IEffectResolver effectResolver,
            PlatoonType platoonType
        )
        {
            _effectResolver = effectResolver;
            PlatoonType = platoonType;

            Step = new UnitStep(5);

            foreach (var statSetup in statConfig)
            {
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));
            }
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

        public void ChangeValue(StatType statType, float value)
        {
            _stats[statType].Modify(value);
        }

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


        [ContextMenu(Constants.AnimationsName.Idle)]
        public void PlayIdleAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.Idle);
            
            AnimatorClipInfo[] clipInfos = _animator.GetCurrentAnimatorClipInfo(0);

            // AnimatorClipInfo[] clipInfos2 = _animator.GetCurrentAnimatorClipInfo(0);
            // var value = clipInfos2[0].clip.name;
            // Debug.Log(value);
            //
            // AnimatorClipInfo[] clipInfos = _animator.GetCurrentAnimatorClipInfo(0);
            // float animationLength = clipInfos[0].clip.length;

            Debug.Log(clipInfos.Length);
        }

        private IEnumerator Timer(float time)
        {
            float startTime = 0;

            while (startTime < time)
            {
                startTime += Time.deltaTime;
                yield return null;
            }

            Debug.Log("Finished");
            Instantiate(ImposedEffect, transform.position, Quaternion.identity).Play();
        }
        
        public float GetDeathAnimationLength(string animationName)
        {
            AnimationClip clip = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == animationName);

            if (clip == null)
                throw new Exception($"Animator not contains animation {animationName}");

            return clip.length;
        }

        [ContextMenu(Constants.AnimationsName.Mage.CastSpell)]
        public void PlayCastAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.Mage.CastSpell);

            // AnimatorClipInfo[] clipInfos = _animator.GetCurrentAnimatorClipInfo(0);
            //
            // AnimatorClipInfo animationClip = clipInfos.FirstOrDefault(x => x.clip.name == "CastSpell");
            
            float lenght = GetDeathAnimationLength(Constants.AnimationsName.Mage.CastSpell);
            Debug.Log(lenght);


            StartCoroutine(Timer(lenght - 0.8f));
        }

        [ContextMenu(Constants.AnimationsName.Mage.FireballAttack)]
        public void PlayFireballAttackkAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.Mage.FireballAttack);
        }

        [ContextMenu(Constants.AnimationsName.Dodge)]
        public void PlayDodgeAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.Dodge);
        }

        [ContextMenu(Constants.AnimationsName.Death)]
        public void PlayDeathAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.Death);
        }

        [ContextMenu(Constants.AnimationsName.TakeDamage)]
        public void PlayApplyDamageAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.TakeDamage);
        }

        [ContextMenu(Constants.AnimationsName.Mage.RadialAttack)]
        public void PlayRadialAttackAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.Mage.RadialAttack);
        }

        [ContextMenu(Constants.AnimationsName.Swordsman.TwoSwordsAttack)]
        public void PlayRadialTwoSwordsAttackAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.Swordsman.TwoSwordsAttack);
        }

        [ContextMenu(Constants.AnimationsName.Archer.MiddleShoot)]
        public void PlayMiddleShootAttackAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.Archer.MiddleShoot);
        }

        [ContextMenu(Constants.AnimationsName.Archer.ShootInSky)]
        public void PlayShootInSkyAnimation()
        {
            _animatorController.PlayAnimation(Constants.AnimationsName.Archer.ShootInSky);
        }
    }
}
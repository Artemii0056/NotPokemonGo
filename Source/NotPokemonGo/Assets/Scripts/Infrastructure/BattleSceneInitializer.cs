using System.Collections;
using System.Collections.Generic;
using Abilities;
using Characters;
using Effects;
using Factories;
using Statuses;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class BattleSceneInitializer : MonoBehaviour
    {
        public UnitFactory UnitFactory;
        
        public AbilityConfig _castamentAbilityConfig;
        public AbilityConfig _armamntAbilityConfig;

        public EffectResolver EffectResolver;

        public UnitStatsPanel UnitStatsPanel;

       // public Unit UnitPrefab;
        public UnitView UnitPrefab;

        public Transform TargetSpawnPoint;
        public Transform SourceSpawnPoint;

        public CharacterStaticData TargetConfig;
        public CharacterStaticData SourceConfig;

        public Unit targetUnit;
        public Unit sourceUnit;

        public Button armamentAbilityButton;
        public Button castamentAbilityButton;

        private EffectManager _effectManager;

        private StatusFactory _statusFactory;
        private AbilityFactory _abilityFactory;
        private AbilityViewFactory _abilityViewFactory;

        private void OnEnable()
        {
            armamentAbilityButton.onClick.AddListener(ArmamentAbilitySpell);
            castamentAbilityButton.onClick.AddListener(CastamentAbilitySpell);
        }

        private void OnDisable()
        {
            armamentAbilityButton.onClick.RemoveListener(ArmamentAbilitySpell);
            castamentAbilityButton.onClick.RemoveListener(CastamentAbilitySpell);
        }

        private void Start()
        {
            EffectResolver = new EffectResolver();
            _statusFactory = new StatusFactory();
            _abilityFactory = new AbilityFactory(_statusFactory);
            _effectManager = new EffectManager();
            _abilityViewFactory = new AbilityViewFactory();

            UnitFactory = new UnitFactory(UnitPrefab);
            

          //  targetUnit = Instantiate(UnitPrefab);
            targetUnit.Construct(TargetConfig.Stats);
            targetUnit.transform.position = TargetSpawnPoint.position;

          //  sourceUnit = Instantiate(UnitPrefab);
            sourceUnit.Construct(SourceConfig.Stats);
            sourceUnit.transform.position = SourceSpawnPoint.position;

            UnitStatsPanel.unit = targetUnit;
        }

        private void CastamentAbilitySpell()
        {
            List<Status> statuses = new List<Status>();
            List<EffectInfo> effects = new List<EffectInfo>();

            Ability ability = _abilityFactory.Create(_castamentAbilityConfig, targetUnit);

            if (ability.HasCastament)
            {
                foreach (var effectSetup in ability.CastamentSetup.EffectsSetup)
                    effects.Add(new EffectInfo(effectSetup.Type, effectSetup.Value));

                foreach (var status in ability.CastamentSetup.Statuses)
                    statuses.Add(_statusFactory.Create(status, targetUnit, EffectResolver));

                ApplyEffectsToTarget(statuses, effects);
            }

            ParticleSystem effect = Instantiate(_castamentAbilityConfig.ParticleSystem);
            effect.transform.position = targetUnit.transform.position;
            effect.Play();
        }

        private void ArmamentAbilitySpell()
        {
            List<Status> statuses = new List<Status>();
            List<EffectInfo> effects = new List<EffectInfo>();

            Ability ability = _abilityFactory.Create(_armamntAbilityConfig, targetUnit);

            if (ability.HasArmament)
                CreateStatsAndEffects(ability, effects, statuses);

            AbilityView abilityView =
                _abilityViewFactory.Create(sourceUnit.abilityPos.position, _armamntAbilityConfig.Prefab, targetUnit);

            StartCoroutine(PlayArmamentAbility(statuses, effects, abilityView));
        }

        private void CreateStatsAndEffects(Ability ability, List<EffectInfo> effects, List<Status> statuses)
        {
            foreach (var effectSetup in ability.ArmamentSetup.EffectsSetup)
                effects.Add(new EffectInfo(effectSetup.Type, effectSetup.Value));

            foreach (var status in ability.ArmamentSetup.Statuses)
                statuses.Add(_statusFactory.Create(status, targetUnit, EffectResolver));
        }

        private IEnumerator PlayArmamentAbility(List<Status> statuses, List<EffectInfo> effects,
            AbilityView abilityView)
        {
            while (Vector3.Distance(targetUnit.transform.position, abilityView.transform.position) > 0.1f)
                yield return null;

            ApplyEffectsToTarget(statuses, effects);
        }

        private void ApplyEffectsToTarget(List<Status> statuses, List<EffectInfo> effects)
        {
            if (statuses.Count > 0)
            {
                foreach (var status in statuses)
                    _effectManager.RegisterStatusEffect(status);
            }

            if (effects.Count > 0)
            {
                foreach (var effectInfo in effects)
                    targetUnit.ReceiveDamage(effectInfo);
            }
        }

        private void Update()
        {
            _effectManager.Update(Time.deltaTime);
            _effectManager.RemoveInactive();
        }
    }
}
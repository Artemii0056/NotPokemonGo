using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Characters;
using DefaultNamespace;
using Effects;
using Factories;
using InputServices;
using Services.StaticDataServices;
using Statuses;
using UI.Ability;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class BattleSceneInitializer : MonoBehaviour
    {
        private StaticDataLoadService _staticDataLoadService;

        public Raycaster raycaster;

        public AbilitiesPanel AbilitiesPanel;

        public UnitFactory UnitFactory;

        public AbilityConfig _castamentAbilityConfig;
        public AbilityConfig _armamntAbilityConfig;

        public EffectResolver EffectResolver;

        public UnitStatsPanel UnitStatsPanel;

        public Unit OriginalUnitPrefab;
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
        private ArmamentViewFactory _armamentViewFactory;

        private void Awake()
        {
            _staticDataLoadService = new StaticDataLoadService();
            AbilitiesPanel.Initialize(_staticDataLoadService);

            EffectResolver = new EffectResolver();
            _statusFactory = new StatusFactory();
            _abilityFactory = new AbilityFactory(_statusFactory);
            _effectManager = new EffectManager();
            _armamentViewFactory = new ArmamentViewFactory();

            UnitFactory = new UnitFactory(UnitPrefab);
        }

        private void OnEnable()
        {
            armamentAbilityButton.onClick.AddListener(ArmamentAbilitySpell);
            castamentAbilityButton.onClick.AddListener(CastamentAbilitySpell);

            raycaster.UnitSearched += OnUnitSearched;
        }

        private void OnDisable()
        {
            armamentAbilityButton.onClick.RemoveListener(ArmamentAbilitySpell);
            castamentAbilityButton.onClick.RemoveListener(CastamentAbilitySpell);

            raycaster.UnitSearched -= OnUnitSearched;
        }

        private void Start()
        {
            targetUnit = Instantiate(OriginalUnitPrefab);
            targetUnit.Initialize(TargetConfig.Stats, EffectResolver);
            targetUnit.transform.position = TargetSpawnPoint.position;
            targetUnit.PlatoonType = PlatoonType.Enemies;

            for (int i = 0; i < TargetConfig.AbilityConfigs.Count; i++)
            {
                targetUnit.AddAbility(new AbilityModel(TargetConfig.AbilityConfigs[i]));
            }

            sourceUnit = Instantiate(OriginalUnitPrefab);
            sourceUnit.Initialize(SourceConfig.Stats, EffectResolver);
            sourceUnit.transform.position = SourceSpawnPoint.position;
            sourceUnit.PlatoonType = PlatoonType.Friends;

            for (int i = 0; i < SourceConfig.AbilityConfigs.Count; i++)
            {
                sourceUnit.AddAbility(new AbilityModel(SourceConfig.AbilityConfigs[i]));
            }

            UnitStatsPanel.unit = targetUnit;
        }

        private void OnUnitSearched(Unit unit)
        {
            if (unit.PlatoonType == PlatoonType.Friends) 
                ShowAbilityInfos(unit.AbilityModels);
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

            ArmamentView armamentView =
                _armamentViewFactory.Create(sourceUnit.abilityPos.position, _armamntAbilityConfig.Prefab, targetUnit);

            StartCoroutine(PlayArmamentAbility(statuses, effects, armamentView));
        }

        private void CreateStatsAndEffects(Ability ability, List<EffectInfo> effects, List<Status> statuses)
        {
            foreach (var effectSetup in ability.ArmamentSetup.EffectsSetup)
                effects.Add(new EffectInfo(effectSetup.Type, effectSetup.Value));

            foreach (var status in ability.ArmamentSetup.Statuses)
                statuses.Add(_statusFactory.Create(status, targetUnit, EffectResolver));
        }

        private IEnumerator PlayArmamentAbility(List<Status> statuses, List<EffectInfo> effects,
            ArmamentView armamentView)
        {
            while (Vector3.Distance(targetUnit.transform.position, armamentView.transform.position) > 0.1f)
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

            targetUnit.Tick(Time.deltaTime);
            sourceUnit.Tick(Time.deltaTime);
        }

        public void ShowAbilityInfos(List<AbilityModel> abilityModels)
        {
            AbilitiesPanel.SetAbilities(abilityModels);
        }
    }
}
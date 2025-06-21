using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Characters;
using Effects;
using Factories;
using InputServices;
using Services;
using Services.StaticDataServices;
using Statuses;
using UI.Ability;
using Units;
using UnityEngine;

namespace Infrastructure
{
    public class BattleSceneInitializer : MonoBehaviour, ICoroutineRunner
    {
        private AbilityApplicatorService _abilityApplicatorService;

        private IStaticDataService _staticDataLoadService;

        public Raycaster raycaster;

        public AbilitiesPanel AbilitiesPanel;

        public UnitFactory UnitFactory;

        private EffectResolver _effectResolver;

        public UnitStatsPanel UnitStatsPanel;

        public Unit OriginalUnitPrefab;

        public Transform TargetSpawnPoint;
        public Transform TargetSpawnPoint2;
        public Transform SourceSpawnPoint;

        public CharacterConfig TargetConfig;
        public CharacterConfig SourceConfig;

        public Unit targetUnit;
        public Unit targetUnit2;
        public Unit sourceUnit;

        private StatusManager _statusManager;

        private StatusFactory _statusFactory;
        private ArmamentViewFactory _armamentViewFactory;

        private void Awake()
        {
            //_staticDataLoadService = new StaticDataService();

            _effectResolver = new EffectResolver();
            _statusFactory = new StatusFactory();

            _statusManager = new StatusManager();
            _armamentViewFactory = new ArmamentViewFactory();
            _abilityApplicatorService = new AbilityApplicatorService(_armamentViewFactory,_statusFactory,_effectResolver, _statusManager, this);
            AbilitiesPanel.Initialize(_staticDataLoadService, _abilityApplicatorService);

            //UnitFactory = new UnitFactory(OriginalUnitPrefab, _effectResolver, _staticDataLoadService);
        }

        private void OnEnable()
        {
            raycaster.UnitSearched += OnUnitSearched;
        }

        private void OnDisable()
        {
            raycaster.UnitSearched -= OnUnitSearched;
        }

        private void Start()
        {
            // targetUnit = UnitFactory.Create(TargetConfig, PlatoonType.Enemies);
            // targetUnit.transform.position = TargetSpawnPoint.position;
            //
            // targetUnit2 = UnitFactory.Create(TargetConfig, PlatoonType.Enemies);
            // targetUnit2.transform.position = TargetSpawnPoint2.position;
            //
            // sourceUnit = UnitFactory.Create(SourceConfig, PlatoonType.Friends);
            // sourceUnit.transform.position = SourceSpawnPoint.position;

            UnitStatsPanel.unit = targetUnit;
        }

        private void OnUnitSearched(Unit unit)
        {
            if (unit.PlatoonType == PlatoonType.Friends)
            {
                ShowAbilityInfos(unit.AbilityModels);
                _abilityApplicatorService.RememberSource(unit);
            }
            else if (unit.PlatoonType == PlatoonType.Enemies)
            {
                _abilityApplicatorService.Apply(unit,targetUnit2);
            }
        }

        private void Update()
        {
            _statusManager.Update(Time.deltaTime);
            _statusManager.RemoveInactive();

            targetUnit.Tick(Time.deltaTime);
            sourceUnit.Tick(Time.deltaTime);
        }

        public void ShowAbilityInfos(List<AbilityModel> abilityModels)
        {
            AbilitiesPanel.SetAbilities(abilityModels);
        }
    }
}
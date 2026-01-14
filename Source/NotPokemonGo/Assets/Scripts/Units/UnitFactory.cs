using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Armaments.Spawner;
using Castaments;
using Characters;
using Platoons;
using ReactionSystems;
using Stats;
using UI;
using UI.Sliders;
using Units.AnimationControllers;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Units
{
    public class UnitFactory : IUnitFactory
    {
        private readonly IObjectResolver _objectResolver;
        private readonly ICastamentApplicator _castamentApplicator;
        private readonly ITargetSelector _targetSelector;
        private readonly IReactionService _reactionService;

        public UnitFactory(
            IObjectResolver objectResolver,
            ICastamentApplicator castamentApplicator,
            ITargetSelector targetSelector,
            IAbilityService abilityService, 
            IReactionService reactionService, 
            IArmamentSpawner spawner)
        {
            _objectResolver = objectResolver;
            _castamentApplicator = castamentApplicator;
            _targetSelector = targetSelector;
            _reactionService = reactionService;

            _reactionService.Register(new ReflectFireballReaction(spawner)); //TODO ВЫПЫЛИТЬ ОТСЮДА! 
            _reactionService.Register(new CounterattackReaction(abilityService)); //TODO ВЫПЫЛИТЬ ОТСЮДА! 
        }

        public Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config, PlatoonType platoonType)
        {
            Vector3 posotion = new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z);

            Unit unit = Object.Instantiate(config.Prefab, posotion, Quaternion.identity);

            unit.transform.SetParent(parentPosition, false);
            
            AnimatorController controller = unit.AnimatorController;

            UnitAnimatorTrigger unitAnimatorTrigger = new UnitAnimatorTrigger(
                unit,
                controller,
                _castamentApplicator,
                _targetSelector, 
                _reactionService);

            unit.SetAnimationTrigger(unitAnimatorTrigger);

            unit.Construct(config.Stats,  platoonType);

            for (int i = 0; i < config.AbilityConfigs.Count; i++)
            {
                unit.AddAbility(new AbilityModel(config.AbilityConfigs[i]));
            }

            InitializeView(unit);

            return unit;
        }
        
        public Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config,  PlatoonType platoonType, Dictionary<StatType, StatSetup> stats)
        {
            Vector3 posotion = new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z);

            Unit unit = Object.Instantiate(config.Prefab, posotion, Quaternion.identity);

            unit.transform.SetParent(parentPosition, false);
            
            unit.Construct(stats, platoonType);

            for (int i = 0; i < config.AbilityConfigs.Count; i++)
            {
                unit.AddAbility(new AbilityModel(config.AbilityConfigs[i]));
            }

            InitializeView(unit);

            return unit;
        }

        private void InitializeView(Unit unit)
        {
            UnitViewPanel unitViewPanel = unit.GetComponentInChildren<UnitViewPanel>();
            UnitSliderView slidersView = unit.GetComponentInChildren<UnitSliderView>();

            _objectResolver.Inject(unitViewPanel);
            _objectResolver.Inject(slidersView);

            unitViewPanel.Construct(unit);
        }
    }
}
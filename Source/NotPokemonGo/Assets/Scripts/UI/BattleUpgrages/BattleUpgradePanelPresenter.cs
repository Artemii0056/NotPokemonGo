using System;
using System.Collections.Generic;
using Abilities;
using Characters;
using Characters.Configs;
using Services.StaticDataServices;
using UnityEngine;
using VContainer.Unity;

namespace UI.BattleUpgrages
{
    public class BattleUpgradePanelPresenter : IStartable
    {
        private BattleUpgradePanel _battleUpgradePanel;
        private IStaticDataService _staticDataService;

        public event Action UpgradeSelected;

        public BattleUpgradePanelPresenter(BattleUpgradePanel battleUpgradePanel, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _battleUpgradePanel = battleUpgradePanel;
        }

        public void Start()
        {
            Disable();
            FillView();
        }

        private void FillView()
        {
        }

        public void Enable(UnitType unitType)
        {
            List<AbilityConfig> result = GetUniqueAbilities(unitType);
            
            _battleUpgradePanel.Initialize(result);

            _battleUpgradePanel.Activate();
            _battleUpgradePanel.UpgradeSelected += OnUpgradeSelected;
        }

        private List<AbilityConfig> GetUniqueAbilities(UnitType unitType)
        {
            List<AbilityConfig> result = new List<AbilityConfig>();
            
            UnitConfig unitConfig = _staticDataService.GetUnitConfig(unitType);
            
            List<AbilityConfig> unitConfigAbility = unitConfig.AbilityConfigs;
            
            List<AbilityConfig> abilityConfigs = _staticDataService.GetAllAbilityConfigs();

            foreach (AbilityConfig abilityConfig in abilityConfigs)
            {
                foreach (AbilityConfig abilityConfig2 in unitConfigAbility)
                {
                    if (abilityConfig2.AbilityType == abilityConfig.AbilityType)
                        continue;
                    
                    result.Add(abilityConfig2);
                }
            }

            return result;
        }

        private void OnUpgradeSelected() => 
            UpgradeSelected?.Invoke();

        public void Disable()
        {
            _battleUpgradePanel.UpgradeSelected -= OnUpgradeSelected;
            _battleUpgradePanel.Deactivate();
        }
    }
}
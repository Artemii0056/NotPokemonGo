using System;
using System.Collections.Generic;
using AbilityNew.Scripts;
using Characters;
using Characters.Configs;
using Services.StaticDataServices;
using VContainer.Unity;

namespace UI.BattleUpgrages
{
    public class BattleUpgradePanelPresenter : IStartable //TODO Дропнуть
    {
        private readonly BattleUpgradePanel _battleUpgradePanel;
        private readonly IStaticDataService _staticDataService;

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
            List<AbilitySo> result = GetUniqueAbilities(unitType);
            
            _battleUpgradePanel.Initialize(result);

            _battleUpgradePanel.Activate();
            _battleUpgradePanel.UpgradeSelected += OnUpgradeSelected;
        }

        private List<AbilitySo> GetUniqueAbilities(UnitType unitType)
        {
            List<AbilitySo> result = new List<AbilitySo>();
            
            UnitConfig unitConfig = _staticDataService.GetUnitConfig(unitType);
            
            List<AbilitySo> unitConfigAbility = unitConfig.AbilitySO;
            
            //List<AbilitySo> abilityConfigs = _staticDataService.GetAllAbilityConfigs();

            // foreach (AbilitySo abilityConfig in abilityConfigs)
            // {
            //     foreach (AbilitySo abilityConfig2 in unitConfigAbility)
            //     {
            //         if (abilityConfig2.Type == abilityConfig.Type)
            //             continue;
            //         
            //         result.Add(abilityConfig2);
            //     }
            // }

            return null;
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
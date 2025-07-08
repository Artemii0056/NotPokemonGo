using System;
using UnityEngine;
using VContainer.Unity;

namespace UI.BattleUpgrages
{
    public class BattleUpgradePanelPresenter : IStartable
    {
        private BattleUpgradePanel _battleUpgradePanel;

        public event Action UpgradeSelected;

        public BattleUpgradePanelPresenter(BattleUpgradePanel battleUpgradePanel)
        {
            _battleUpgradePanel = battleUpgradePanel;
        }

        public void Start()
        {
            Disable();
        }

        public void Enable()
        {
            _battleUpgradePanel.Activate();
            _battleUpgradePanel.UpgradeSelected += OnUpgradeSelected;
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
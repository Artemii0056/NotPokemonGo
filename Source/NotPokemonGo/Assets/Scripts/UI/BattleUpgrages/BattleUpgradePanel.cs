using System;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

namespace UI.BattleUpgrages
{
    public class BattleUpgradePanel : MonoBehaviour
    {
        [SerializeField] private List<BattleUpgradeButton> _battleUpgradeButtons;

        public event Action UpgradeSelected;

        public void Initialize(List<AbilityConfig> abilityConfigs)
        {
            for (int i = 0; i < _battleUpgradeButtons.Count; i++)
            {
                if (i >= abilityConfigs.Count - 1)
                    return;
                
                _battleUpgradeButtons[i].Initialize(abilityConfigs[i].Icon, abilityConfigs[i].AbilityType);
            }
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            
            foreach (BattleUpgradeButton button in _battleUpgradeButtons) 
                button.UpgradeSelected += OnUpgradeSelected;
        }

        public void Deactivate()
        {
            foreach (BattleUpgradeButton button in _battleUpgradeButtons) 
                button.UpgradeSelected -= OnUpgradeSelected;

            gameObject.SetActive(false);
        }

        private void OnUpgradeSelected() => 
            UpgradeSelected?.Invoke();
    }
}
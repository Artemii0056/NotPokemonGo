using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.BattleUpgrages
{
    public class BattleUpgradePanel : MonoBehaviour
    {
        [SerializeField] private List<BattleUpgradeButton> _battleUpgradeButtons;
        public event Action UpgradeSelected;
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
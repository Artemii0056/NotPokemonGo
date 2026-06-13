using System.Collections.Generic;
using Abilities.MV;

namespace UI.Ability
{
    public class AbilityPanelPresenter //TODO А нахера
    {
        private AbilitiesPanel _abilitiesPanel;

        public AbilityPanelPresenter(AbilitiesPanel abilitiesPanel)
        {
            _abilitiesPanel = abilitiesPanel;
        }

        public void Enable()
        {
            _abilitiesPanel.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _abilitiesPanel.Hide();
            //_abilitiesPanel.gameObject.SetActive(false);
        }

        public void FillAbilityView(List<AbilityModel> abilityModels) => 
            _abilitiesPanel.SetAbilities(abilityModels);
    }
}
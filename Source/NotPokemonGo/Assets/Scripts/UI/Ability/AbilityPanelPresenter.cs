using System.Collections.Generic;
using Abilities.MV;
using Infrastructure.MVP.Implementation;
using VContainer.Unity;

namespace UI.Ability
{
    public class AbilityPanelPresenter : IPresenter
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
            _abilitiesPanel.gameObject.SetActive(false);
        }

        public void FillAbilityView(List<AbilityModel> abilityModels)
        {
            _abilitiesPanel.SetAbilities(abilityModels);
        }
    }
}
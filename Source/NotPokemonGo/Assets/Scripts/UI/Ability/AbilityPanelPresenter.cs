using Infrastructure.MVP.Implementation;

namespace UI.Ability
{
    public class AbilityPanelPresenter : IPresenter
    {
        private AbilitiesPanel _abilitiesPanel;

        public AbilityPanelPresenter(AbilitiesPanel abilitiesPanel)
        {
            _abilitiesPanel = abilitiesPanel;
            _abilitiesPanel.gameObject.SetActive(false);
        }
        
        public void Enable()
        {
            _abilitiesPanel.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _abilitiesPanel.gameObject.SetActive(false);
        }
    }
}
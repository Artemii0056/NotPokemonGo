using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Services.StaticDataServices;
using UnityEngine;

namespace UI.Ability
{
    public class AbilitiesPanel : MonoBehaviour
    {
        [SerializeField] private List<AbilityView> _abilitiesView;

        private StaticDataLoadService _staticDataLoadService;
        private AbilityApplicatorService _abilityApplicatorService;

        public void Initialize(StaticDataLoadService staticDataLoadService, AbilityApplicatorService abilityApplicatorService)
        {
            _staticDataLoadService = staticDataLoadService;
            _abilityApplicatorService = abilityApplicatorService;
        }

        public void SetAbilities(List<AbilityModel> abilityModels)
        {
            for (int i = 0; i < abilityModels.Capacity; i++)
            {
                _abilitiesView[i].Initialize(abilityModels[i]);

                AbilityConfig config = _staticDataLoadService.GetAbilityConfig(abilityModels[i].AbilityType);

                _abilitiesView[i].SetImage(config.Icon);
            }

            for (int i = abilityModels.Capacity; i < _abilitiesView.Capacity; i++)
            {
                _abilitiesView[i].SetDefaultImage();
            }

            foreach (var view in _abilitiesView)
            {
                view.InitService(_abilityApplicatorService);
            }
        }
    }
}
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Services.StaticDataServices;
using UnityEngine;
using VContainer;

namespace UI.Ability
{
    public class AbilitiesPanel : MonoBehaviour
    {
        [SerializeField] private List<AbilityView> _abilitiesView;

        private IStaticDataService _staticDataLoadService;
        private IObjectResolver _objectResolver;

        [Inject]
        public void Initialize(IStaticDataService staticDataLoadService, IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
            _staticDataLoadService = staticDataLoadService;
        }

        public void Tick(float deltaTime)
        {
            foreach (AbilityView abilityView in _abilitiesView)
            {
                abilityView.Tick(deltaTime);
            }
        }
        
        public void SetAbilities(List<AbilityModel> abilityModels)
        {
            for (int i = 0; i < abilityModels.Capacity; i++)
            {
                _abilitiesView[i].Construct(abilityModels[i]);

                AbilityConfig config = _staticDataLoadService.GetAbilityConfig(abilityModels[i].AbilityType);

                _abilitiesView[i].SetImage(config.Icon);
            }

            for (int i = abilityModels.Capacity; i < _abilitiesView.Capacity; i++) 
                _abilitiesView[i].SetDefaultImage();

            foreach (AbilityView view in _abilitiesView) 
                _objectResolver.Inject(view);
        }
    }
}
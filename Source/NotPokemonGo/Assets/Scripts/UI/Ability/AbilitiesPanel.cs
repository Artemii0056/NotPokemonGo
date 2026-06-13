using System;
using System.Collections.Generic;
using Abilities.MV;
using UnityEngine;
using VContainer;

namespace UI.Ability
{
    public class AbilitiesPanel : MonoBehaviour
    {
        [SerializeField] private List<AbilityView> _abilitiesView;

        private IObjectResolver _objectResolver;
        
        public event Action<AbilityModel> AbilityModelSelected;

        [Inject]
        public void Initialize(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
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
            for (int i = 0; i < abilityModels.Count; i++)
            {
                _abilitiesView[i].Construct(abilityModels[i]);

                _abilitiesView[i].SetImage(abilityModels[i].ConfigSO.Icon);
            }

            for (int i = abilityModels.Count; i < _abilitiesView.Count; i++) 
                _abilitiesView[i].SetDefaultImage();
            
            foreach (AbilityView view in _abilitiesView) 
                view.OnAbility += OnAbilitySelected;

            foreach (AbilityView view in _abilitiesView) //TODO Тоже херня
                _objectResolver.Inject(view);
        }

        private void OnAbilitySelected(AbilityModel model)
        {
            AbilityModelSelected?.Invoke(model);
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            foreach (var view in _abilitiesView) 
                view.OnAbility -= OnAbilitySelected;
        }
    }
}
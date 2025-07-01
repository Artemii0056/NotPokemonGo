using System;
using Abilities.MV;
using Units.AnimationControllers;

namespace Characters
{
    public class UnitStep
    {
        private  AbilityModel _abilityModel;
        private UnitAnimatorTrigger _unitAnimatorTrigger;

        public UnitStep(UnitAnimatorTrigger unitAnimatorTrigger)
        {
            _unitAnimatorTrigger = unitAnimatorTrigger;
        }

        public event Action<AbilityModel> OnAbility;
        
        public event Action ActionEnded;

        public void SetAbilityModel(AbilityModel abilityModel)
        {
            _abilityModel = abilityModel;
        }
        
        //Должен работать с UnitAnimatorTrigger
        
        //установили абилку
        //условный метод Run
        //проверить тип Фазы
        //проиграть логику
        //проверить есть ли дальше фаза
        //нет - кинуть событие
        
        
    }
}
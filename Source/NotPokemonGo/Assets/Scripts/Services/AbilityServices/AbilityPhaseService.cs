using Abilities;
using Abilities.AbilityActions.Castaments;
using Abilities.MV;
using Effects;
using Infrastructure.ReactionSystem;
using ReactionSystems;
using Statuses.Services;
using Units;
using UnityEngine;

namespace Services.AbilityServices
{
    public class AbilityPhaseService
    {
        private readonly IAbilityApplicatorService _abilityApplicatorService;
        private readonly ITargetSelector _targetSelector;
        private readonly IReactionService _reactionService;

        public AbilityPhaseService(
            IAbilityApplicatorService abilityApplicatorService, 
            ITargetSelector targetSelector, 
            IReactionService reactionService)
        {
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;
            _reactionService = reactionService;
        }

        public void OnNext(AbilityPhase phase, Unit source) //Тут сделать два метода 
        {
                Unit target = _targetSelector.Target;

                var setup = phase.CastamentSetup;

                if (setup.HasSetupData == false)
                    return;

                var context = new ReactionContext(source, target, setup.EffectsSetup[0], phase);

                Debug.Log("Тут был вообще? ");
                
                if (_reactionService.TryReact(context)) //Сюда не зашел. А, нужно реакции забиндить
                    return;

                _abilityApplicatorService.Apply(setup, _targetSelector.GetTargets(phase.TargetMode).ToArray());
            
            if (phase.ArmamentSetup.HasSetupData) 
                _abilityApplicatorService.Apply(phase.ArmamentSetup, _targetSelector.GetTargets(phase.TargetMode).ToArray());
        }

        private void HandleCastamentSetup(CastamentSetup setup)
        {
            
        }

        /* public void OnNext(AbilityPhase phase)
        {
            Unit target = _targetSelector.Target;

            CastamentSetup setup = phase.CastamentSetup;

            if (setup.HasSetupData && phase.PhaseType == PhaseType.IsMelee)
            {
                if (setup.EffectsSetup[0].DamageType == DamageType.Physical)
                {
                    if ( SearchCounterAttackAbility(target, out AbilityModel abilityModel))
                    {
                        _abilityService.HandleCounterAttack(target, abilityModel);
                        return;
                    }

                    //и тут сервис? Ебаниииина
                }

                _abilityApplicatorService.Apply(setup, _targetSelector.GetTargets(phase.TargetMode).ToArray());
            }


            if (phase.ArmamentSetup.HasSetupData) //На армамент пофигу?
                _abilityApplicatorService.Apply(phase.ArmamentSetup, _targetSelector.GetTargets(phase.TargetMode).ToArray());
        }*/
        
        private bool SearchCounterAttackAbility(Unit target, out AbilityModel ability)
        {
            ability = null;

            foreach (var abilityModel in target.AbilityModels)
            {
                if (abilityModel.AbilityType == AbilityType.CounterAttack && abilityModel.IsReady())
                {
                    ability = abilityModel;
                    return true;
                }
            }

            return false;
        }
    }
}
using Abilities;
using Abilities.MV;
using Effects;
using Infrastructure.ReactionSystem;
using UnityEngine;

namespace ReactionSystems
{
    public class CounterattackReaction : IReaction
    {
        private readonly IAbilityService _abilityService;

        public CounterattackReaction(IAbilityService abilityService)
        {
            _abilityService = abilityService;
        }

        public bool CanReact(ReactionContext context)
        {
            Debug.Log("CounterattackReaction canReact");
            
            if (context.Effect.Type != EffectType.Damage)
                return false;
            
            Debug.Log("CounterattackReaction Эффект тайп");

            if (context.Effect.DamageType != DamageType.Physical)
                return false;
            
            Debug.Log("CounterattackReaction damageType");

            return context.Target.AbilityModels.Exists(a =>
                a.AbilityType == AbilityType.CounterAttack && a.IsReady());
        }

        public void React(ReactionContext context)
        {
            AbilityModel ability = context.Target.AbilityModels
                .Find(a => a.AbilityType == AbilityType.CounterAttack && a.IsReady());

            Debug.Log("Counterattack reaction false");
            if (ability != null)
            {
                Debug.Log("Counterattack reaction received");
                _abilityService.HandleCounterAttack(context.Target, ability);
            }
        }
    }
}
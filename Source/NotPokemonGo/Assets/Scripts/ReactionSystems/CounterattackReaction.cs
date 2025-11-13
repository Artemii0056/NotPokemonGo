using Abilities;
using Abilities.MV;
using Effects;
using Infrastructure.ReactionSystem;

namespace ReactionSystems
{
    public class CounterattackReaction : IReaction
    {
        private readonly IAbilityService _abilityService;

        public CounterattackReaction(IAbilityService abilityService) => 
            _abilityService = abilityService;

        public bool CanReact(ReactionContext context)
        {
            if (context.Effect.Type != EffectType.Damage)
                return false;
            
            if (context.Effect.DamageType != DamageType.Physical)
                return false;
            
            return context.Target.AbilityModels.Exists(a =>
                a.AbilityType == AbilityType.CounterAttack && a.IsReady());
        }

        public void React(ReactionContext context)
        {
            AbilityModel ability = context.Target.AbilityModels
                .Find(a => a.AbilityType == AbilityType.CounterAttack && a.IsReady());

            if (ability != null) 
                _abilityService.HandleCounterAttack(context.Source,context.Target, ability);
        }
    }
}
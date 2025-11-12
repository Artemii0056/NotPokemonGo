using System;
using Abilities.MV;
using Units;

namespace Abilities
{
    public interface IAbilityService
    {
        void Handle(Unit source, Unit target, Battlefield battlefield, AbilityModel abilityModel);
        event Action Finished;
        void HandleCounterAttack(Unit target, AbilityModel abilityModel);
    }
}
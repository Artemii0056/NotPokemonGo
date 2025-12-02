using System;
using Abilities.MV;
using Battlefields;
using Units;

namespace Abilities
{
    public interface IAbilityService
    {
        void Handle(Unit source, Unit target, AbilityModel abilityModel);
        void SetBattlefield(Battlefield battlefield);
        void HandleCounterAttack(Unit source,Unit target, AbilityModel abilityModel);
        event Action Finished;
    }
}
using System;
using Abilities.MV;
using Units;

namespace Abilities
{
    public interface IAbilityService
    {
        void Handle(Unit source, Unit target, AbilityModel abilityModel);
        void SetBattlefield(Battlefield battlefield);
        event Action Finished;
    }
}
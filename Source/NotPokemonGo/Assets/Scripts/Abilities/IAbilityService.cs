using System;
using Abilities.MV;
using Battlefields;
using Cysharp.Threading.Tasks;
using Units;

namespace Abilities
{
    public interface IAbilityService
    {
        UniTask RunAbilityAsync (Unit source, Unit target, AbilityModel abilityModel);
        void SetBattlefield(Battlefield battlefield);
        event Action Finished;
    }
}
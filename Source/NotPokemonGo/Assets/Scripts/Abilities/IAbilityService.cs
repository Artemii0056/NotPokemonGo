using System;
using Units;

namespace Abilities
{
    public interface IAbilityService
    {
        void Handle(Unit source, Unit target, Battlefield battlefield);
        event Action Finished;
    }
}
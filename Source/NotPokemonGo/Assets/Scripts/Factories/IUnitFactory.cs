using Characters;
using Units;
using UnityEngine;

namespace Factories
{
    public interface IUnitFactory
    {
        Unit Create(Transform position, Transform parentPosition, CharacterConfig config, PlatoonType platoonType);
    }
}
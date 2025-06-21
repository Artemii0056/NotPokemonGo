using Characters;
using Units;
using UnityEngine;

namespace Factories
{
    public interface IUnitFactory
    {
        Unit Create(Vector3 spawnPosition, Transform parentPosition, CharacterConfig config, PlatoonType platoonType);
    }
}
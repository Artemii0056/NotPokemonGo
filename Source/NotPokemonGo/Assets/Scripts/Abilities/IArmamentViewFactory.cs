using Units;
using UnityEngine;

namespace Abilities
{
    public interface IArmamentViewFactory
    {
        ArmamentView Create(Vector3 position, ArmamentView armamentConfigPrefab, Unit targetUnit);
    }
}
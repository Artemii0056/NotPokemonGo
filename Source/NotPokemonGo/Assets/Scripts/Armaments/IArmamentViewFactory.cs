using Units;
using UnityEngine;

namespace Armaments
{
    public interface IArmamentViewFactory
    {
        ArmamentView Create(Vector3 position, ArmamentView armamentConfigPrefab, Unit targetUnit);
    }
}
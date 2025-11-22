using Units;
using UnityEngine;

namespace Armaments.Factories
{
    public class ArmamentViewFactory : IArmamentViewFactory
    {
        public ArmamentView Create(Vector3 position, ArmamentView armamentConfigPrefab, Unit targetUnit)
        {
            ArmamentView armamentView = Object.Instantiate(armamentConfigPrefab, position, Quaternion.identity);
            armamentView.Initialize(targetUnit);
            
            return armamentView;
        }
    }
}
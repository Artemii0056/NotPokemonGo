using Armaments;
using UnityEngine;

namespace Factories.ArmamentViewFactories
{
    public interface IArmamentViewFactory
    {
        Armament Create(ArmamentContext context, Transform transform);
    }
}
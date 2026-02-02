using Armaments;

namespace Factories.ArmamentViewFactories
{
    public interface IArmamentViewFactory
    {
        Armament Create(ArmamentContext context);
    }
}
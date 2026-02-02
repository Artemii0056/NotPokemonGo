using Armaments;
using Armaments.Movers;

namespace Spawners.Spawner
{
    public interface IArmamentSpawner
    {
        IArmamentMover Create(ArmamentContext context);
    }
}
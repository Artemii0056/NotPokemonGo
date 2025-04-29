using TestECS.Gameplay.Features.Statuses;

namespace TestECS.Gameplay.Features.Applier
{
    public interface IStatusApplier
    {
        GameEntity ApplyStatus(StatusSetup statusSetup, int producerId, int targetId);
    }
}
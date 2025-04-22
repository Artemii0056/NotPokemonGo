using Infrastructure.Systems;
using TestECS.Gameplay.Features.Movement.Systems;

namespace TestECS.Gameplay.Features.Movement.Features
{
    public class MovementFeature : Feature
    {
        public MovementFeature(ISystemFactory factory)
        {
            Add(factory.Create<MoveByDirection>());
            Add(factory.Create<UpdateTransformPositionSystem>());
        }
    }
}
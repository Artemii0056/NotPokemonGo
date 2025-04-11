using Entitas;
using TestECS.Gameplay.Code.Features.Movement.Systems;

namespace TestECS.Gameplay.Code.Features
{
    public class MovementFeature : Feature
    {
        public MovementFeature(GameContext gameContext, ITimeService timeService)
        {
            Add(new DirectionalDeltaMoveSystem(gameContext, timeService));
        }
    }
}
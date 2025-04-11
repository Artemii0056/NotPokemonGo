using TestECS.Gameplay.Code.Features;
using TestECS.Gameplay.Code.Features.Movement.Systems;
using TestECS.Gameplay.Input;
using TestECS.Gameplay.Input.Systems;

namespace Code.Gameplay
{
    public class GameplayFeature : Feature
    {
        public GameplayFeature(GameContext gameContext, ITimeService timeService, IInputService inputService)
        {
            Add(new InputFeature(gameContext, inputService));
            Add(new MovementFeature(gameContext, timeService));
            
        }
    }
}
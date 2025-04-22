using Infrastructure.Systems;
using TestECS.Gameplay.Input.Systems;

namespace TestECS.Gameplay.Input
{
    public class InputFeature : Feature
    {
        public InputFeature(ISystemFactory factory)
        {
            Add(factory.Create<InitializeInputSystem>());
            Add(factory.Create<EmitInputSystem>());
        }
    }
}
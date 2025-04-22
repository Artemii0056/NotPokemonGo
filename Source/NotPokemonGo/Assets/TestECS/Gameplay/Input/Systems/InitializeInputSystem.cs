using Code.Entity;
using Entitas;

namespace TestECS.Gameplay.Input.Systems
{
    public class InitializeInputSystem : IInitializeSystem
    {
        public void Initialize()
        {
            CreateEntity.Empty()
                .isInput = true;
        }
    }
}
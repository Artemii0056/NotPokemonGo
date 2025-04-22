using Entitas;
using TestECS.Gameplay.Input.Service;
using UnityEngine;

namespace TestECS.Gameplay.Input.Systems
{
    public class EmitInputSystem : IExecuteSystem
    {
        private readonly IInputService _inputService;
        private readonly IGroup<GameEntity> _inputs;

        public EmitInputSystem(GameContext context, IInputService inputService)
        {
            _inputService = inputService;
            
            _inputs = context.GetGroup(GameMatcher.Input);
        }
        
        public void Execute()
        {
            if (_inputs.GetEntities().Length == 0)
                return;
            
            foreach (var input in _inputs)
            {
                if (_inputService.HasAxisInput())
                    input.ReplaceAxisInput(new Vector2(_inputService.GetHorizontalAxis(), _inputService.GetVerticalAxis()));
                else if(input.hasAxisInput)
                    input.RemoveAxisInput();
            }
        }
    }
}
using System;
using Code.Entity;
using Entitas;
using UnityEngine;

namespace TestECS.Gameplay.Input.Systems
{
    public class InputFeature : Feature
    {
        public InputFeature(GameContext context, IInputService inputService)
        {
            Add(new InitializeInputSystem());
            Add(new EmitInputSystem(context, inputService));
            Add(new TestSystem(inputService));
        }
    }

    public class TestSystem : IExecuteSystem
    {
        private readonly IInputService _inputService;

        public TestSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Execute()
        {
            if (_inputService.GetLeftMouseButtonDown())
            {
                 CreateEntity.
                    Empty()
                    .AddId(1)
                    .AddSpeed(2)
                    .AddWorldPosition(default)
                    .AddDirection(default);

                 Debug.Log("Created");
            }
        }
    }
}
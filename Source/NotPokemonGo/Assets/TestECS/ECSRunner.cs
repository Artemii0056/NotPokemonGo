using System;
using Code.Gameplay;
using TestECS.Gameplay;
using TestECS.Gameplay.Code.Features.Movement.Systems;
using TestECS.Gameplay.Input;
using UnityEngine;
using Zenject;

namespace TestECS
{
    public class ECSRunner : MonoBehaviour
    {
        private GameContext _gameContext;
        private ITimeService _timeService;
        private IInputService _inputService;

        private GameplayFeature _gameplayFeature;

        [Inject]
        public void Construct(GameContext gameContext, ITimeService timeService, IInputService inputService)
        {
            _gameContext = gameContext;
            _timeService = timeService;
            _inputService = inputService;
        }
        
        private void Start()
        {
            _gameplayFeature = new GameplayFeature(_gameContext, _timeService, _inputService);
            _gameplayFeature.Initialize();
        }

        private void Update()
        {
            _gameplayFeature.Execute();
            _gameplayFeature.Cleanup();
        }

        private void OnDestroy()
        {
            _gameplayFeature.TearDown();
        }
    }
}
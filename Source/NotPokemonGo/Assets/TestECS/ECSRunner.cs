using Code.Gameplay;
using Infrastructure.Systems;
using UnityEngine;
using Zenject;

namespace TestECS
{
    public class ECSRunner : MonoBehaviour
    {
        private GameplayFeature _gameplayFeature;
        private ISystemFactory _systemFactory;

        [Inject]
        public void Construct(ISystemFactory systemsFactory) => 
            _systemFactory = systemsFactory;

        private void Start()
        {
            _gameplayFeature = _systemFactory.Create<GameplayFeature>();
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
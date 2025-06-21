using Infrastructure.DI.Initializers.Scenes;
using Infrastructure.DI.Scopes;
using UI.SpawnPositions;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Installers.Scenes
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private SpawnPositionView _spawnPositionView;
        [SerializeField] private MainMenuInitializer _mainMenuInitializer;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterComponent(_spawnPositionView);
            
            builder.Register<SpawnPositionPresenter>(Lifetime.Singleton);
            builder.RegisterComponent(_mainMenuInitializer).AsImplementedInterfaces();
        }
    }
}
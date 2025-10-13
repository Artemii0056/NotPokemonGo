using Infrastructure.DI.Initializers.Scenes;
using Infrastructure.DI.Scopes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Installers.Scenes
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuInitializer _mainMenuInitializer;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterComponent(_mainMenuInitializer).AsImplementedInterfaces();
        }
    }
}
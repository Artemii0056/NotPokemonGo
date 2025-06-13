using Infrastructure.DI.Initializers;
using Infrastructure.DI.Scopes;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Services.AssetManagement;
using Services.SceneServices;
using Services.StatesServices;
using Services.StaticDataServices;
using Services.SystemFactoryServices;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Installers
{
    public class GlobalServiceInstaller : MonoInstaller
    {
         [SerializeField] private GameScopeInitializer _gameScopeInitializer;
        public override void Install(IContainerBuilder builder)
        {
            RegisterGameStateMachine(builder);
            builder.RegisterComponent(_gameScopeInitializer).AsImplementedInterfaces();
            

            RegisterStates(builder);
            RegisterServices(builder);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IResourceLoader, ResourceLoader>(Lifetime.Singleton);
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            builder.Register<ISystemFactory, SystemFactory>(Lifetime.Singleton);
            builder.Register<IStaticDataLoadService, StaticDataLoadService>(Lifetime.Singleton);
        }

        private void RegisterGameStateMachine(IContainerBuilder builder)
        {
            builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<IStateProvider, StateProvider>(Lifetime.Singleton);
            
            builder.Register<BootstrapState>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Register<LoadMainMenuState>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}
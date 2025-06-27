using Abilities;
using Animations;
using Battlefields;
using Effects;
using Factories;
using Infrastructure.DI.Initializers.Globals;
using Infrastructure.DI.Scopes;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Infrastructure.StateMachines.States;
using InputServices;
using Platoons;
using Services;
using Services.AssetManagement;
using Services.Cameras;
using Services.SceneServices;
using Services.StatesServices;
using Services.StaticDataServices;
using Services.SystemFactoryServices;
using Statuses.Services;
using UI.Factory;
using Units.AnimationControllers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Installers.Gloabals
{
    public class GlobalServiceInstaller : MonoInstaller
    {
         [SerializeField] private GameScopeInitializer _gameScopeInitializer;
         [SerializeField] private InputReader _inputReader;
        public override void Install(IContainerBuilder builder)
        {
            RegisterGameStateMachines(builder);
            
            builder.RegisterComponent(_gameScopeInitializer).AsImplementedInterfaces();
            builder.RegisterComponent(_inputReader).AsImplementedInterfaces();
            
            RegisterStates(builder);
            RegisterServices(builder);
            RegisterFactories(builder);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);
            builder.Register<IUnitFactory, UnitFactory>(Lifetime.Singleton);
            builder.Register<IPlatoonFactory, PlatoonFactory>(Lifetime.Singleton);
            builder.Register<IBattlefieldFactory, BattlefieldFactory>(Lifetime.Singleton);
            builder.Register<IArmamentViewFactory, ArmamentViewFactory>(Lifetime.Singleton);
            builder.Register<IStatusFactory, StatusFactory>(Lifetime.Singleton);
            builder.Register<ISystemFactory, SystemFactory>(Lifetime.Singleton);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IResourceLoader, ResourceLoader>(Lifetime.Singleton);
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            builder.Register<IStaticDataService, StaticDataService>(Lifetime.Singleton);
            builder.Register<IEffectResolver, EffectResolver>(Lifetime.Singleton);
            builder.Register<IStatusResolver, StatusResolver>(Lifetime.Singleton);
            builder.Register<IStatusManager, StatusManager>(Lifetime.Singleton);
            builder.Register<IAbilityApplicatorService, AbilityApplicatorService>(Lifetime.Singleton);
            builder.Register<IRaycaster, Raycaster>(Lifetime.Singleton);
            builder.Register<IParticleSystemFactory, ParticleSystemFactory>(Lifetime.Singleton);
            
            
            builder.Register<ISourceProvider, SourceProvider>(Lifetime.Singleton);
            builder.Register<IAbilityProvider, AbilityProvider>(Lifetime.Singleton);
            builder.Register<ITargetSelector, TargetSelector>(Lifetime.Singleton);
            
            builder.Register<ICameraProvider, CameraProvider>(Lifetime.Singleton);
        }

        private void RegisterGameStateMachines(IContainerBuilder builder)
        {
            builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
            builder.Register<IBattleStateMachine, BattleStateMachine>(Lifetime.Singleton);
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
            
            builder.Register<LoadingBattleState>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();
            
            builder.Register<BattleLoopState>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}
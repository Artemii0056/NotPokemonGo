using Abilities;
using Battlefields;
using Effects;
using Factories;
using Infrastructure.DI.Initializers.Globals;
using Infrastructure.DI.Scopes;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using InputServices;
using Platoons;
using Services.AssetManagement;
using Services.BattleUnitContainers;
using Services.Cameras;
using Services.InputServices;
using Services.QTEServices;
using Services.SceneServices;
using Services.StatesServices;
using Services.StaticDataServices;
using Services.SystemFactoryServices;
using Statuses.Services;
using UI.Ability;
using UI.BattleUpgrages;
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
         [SerializeField] private AbilitiesPanel _abilitiesPanel;
         [SerializeField] private BattleUpgradePanel _battleUpgradePanel;
         
        public override void Install(IContainerBuilder builder)
        {
            RegisterGameStateMachines(builder);
            
            builder.RegisterComponent(_gameScopeInitializer).AsImplementedInterfaces();
            builder.RegisterComponent(_inputReader).AsImplementedInterfaces();
            
            RegisterUserInterface(builder);
            RegisterStates(builder);
            RegisterServices(builder);
            RegisterFactories(builder);
        }

        private void RegisterUserInterface(IContainerBuilder builder)
        {
            builder.RegisterComponent(_abilitiesPanel).AsImplementedInterfaces();
            builder.Register<AbilityPanelPresenter>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();
            
            builder.RegisterComponent(_battleUpgradePanel).AsImplementedInterfaces();
            builder.Register<BattleUpgradePanelPresenter>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();
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
            builder.Register<IRaycasterService, RaycasterServiceService>(Lifetime.Singleton);
            builder.Register<IQTEService, QTEService>(Lifetime.Singleton);
            
            builder.Register<IParticleSystemFactory, ParticleSystemFactory>(Lifetime.Singleton);
            
            
            builder.Register<ISourceProvider, SourceProvider>(Lifetime.Singleton);
            builder.Register<IAbilityProvider, AbilityProvider>(Lifetime.Singleton);
            builder.Register<ITargetSelector, TargetSelector>(Lifetime.Singleton);
            
            builder.Register<ICameraProvider, CameraProvider>(Lifetime.Singleton);

            builder.Register<IBattleUnitContainer, BattleUnitContainer>(Lifetime.Singleton);
        }

        private void RegisterGameStateMachines(IContainerBuilder builder)
        {
            builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
            builder.Register<IBattleStateMachine, BattleStateMachine>(Lifetime.Singleton);
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<IStateProvider, StateProvider>(Lifetime.Singleton);
            
            RegisterGlobalStates(builder);
            RegisterBattleStates(builder);
            RegisterUIStates(builder);
            void RegisterGlobalStates(IContainerBuilder builder)
            {
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

            void RegisterUIStates(IContainerBuilder builder)
            {
                builder.Register<StartScreenState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();
                
                builder.Register<ShowHeroState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();
                
                builder.Register<ChooseMapState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();
                
                builder.Register<ChooseUnitToFightState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();
            }

            void RegisterBattleStates(IContainerBuilder builder)
            {
                builder.Register<UnitActionState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();
                
                builder.Register<UpdateBattleTickState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();

                builder.Register<SelectReadyUnitState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();

                builder.Register<BattleUpgradeSelectionState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();

                builder.Register<FinishBattleState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();

                builder.Register<QTEBattleState>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();
            }
        }
    }
}
using Code.Gameplay.Common.Collisions;
using Code.Gameplay.Common.GamePhysics._2D;
using Code.Gameplay.Common.GamePhysics._3D;
using Code.Infrastructure.AssetManagement;
using Infrastructure.Systems;
using Infrastructure.View.Systems.Factory;
using TestECS.Gameplay.Code.Common.EntityIndices;
using TestECS.Gameplay.Enemies.Factory;
using TestECS.Gameplay.Features.Abilities.Factory;
using TestECS.Gameplay.Features.Applier;
using TestECS.Gameplay.Features.Armaments.Factory;
using TestECS.Gameplay.Features.Effects.Factory;
using TestECS.Gameplay.Features.Statuses.Factory;
using TestECS.Gameplay.Hero.Factory;
using TestECS.Gameplay.Hero.Registrars;
using TestECS.Gameplay.Input.Service;
using TestECS.Levels;
using TestECS.Services.StaticData;
using TestECS.TimeService;
using UnityEditor;
using Zenject;

namespace Infrastructure.Zenject
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BootstrapInstaller>().FromInstance(this).AsSingle();
            
            Container.Bind<ISystemFactory>().To<SystemFactory>().AsSingle();
            Container.Bind<IPhysicsService2D>().To<PhysicsService2D>().AsSingle();
            Container.Bind<IPhysicsService3D>().To<PhysicsService3D>().AsSingle();
            
            Container.Bind<ILevelDataProvider>().To<LevelDataProvider>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            
            Container.Bind<ICollisionRegistry>().To<CollisionRegistry>().AsSingle();
            Container.Bind<IIdService>().To<IdentifierService>().AsSingle();
            
            Container.Bind<ITimeService>().To<TimeService>().AsSingle();
            Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
            Container.Bind<GameContext>().FromInstance(Contexts.sharedInstance.game).AsSingle();
            Container.Bind<Contexts>().FromInstance(Contexts.sharedInstance).AsSingle();
            
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameEntityIndices>().AsSingle();
            
            Container.Bind<IStatusApplier>().To<StatusApplier>().AsSingle();
            
            
            BindGameplayFactories();
            
            //бинд через frominstance
           // Container.BindInterfacesAndSelfTo<TimeService>().AsSingle(); //Bind - регистрация. 
        }

        private void BindGameplayFactories()
        {
            Container.Bind<IEffectFactory>().To<EffectFactory>().AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
            Container.Bind<IEntityViewFactory>().To<EntityViewFactory>().AsSingle();
            Container.Bind<IArmamentsFactory>().To<ArmamentsFactory>().AsSingle();
            Container.Bind<IAbilityFactory>().To<AbilityFactory>().AsSingle();
            Container.Bind<IStatusFactory>().To<StatusFactory>().AsSingle();
        }
    }
}
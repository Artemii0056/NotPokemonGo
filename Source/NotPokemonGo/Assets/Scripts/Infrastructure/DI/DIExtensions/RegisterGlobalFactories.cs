using Armaments;
using Battlefields;
using Effects.Factory;
using Factories.ArmamentViewFactories;
using Factories.PlatoonFactories;
using Platoons;
using Services.BattleUnitContainers;
using Services.StatesServices;
using Services.SystemFactoryServices;
using Spawners;
using Spawners.Spawner;
using Statuses;
using Statuses.Factory;
using UI.Factory;
using Units;
using VContainer;

namespace Infrastructure.DI.DIExtensions
{
	public static partial class ContainerBuilderExtensions
	{
		public static IContainerBuilder RegisterGlobalFactories(this IContainerBuilder builder)
		{
			builder.Register<IStateFactory, StateFactory>(Lifetime.Singleton);

			builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);
			builder.Register<IUnitFactory, UnitFactory>(Lifetime.Singleton);
			builder.Register<IPlatoonFactory, PlatoonFactory>(Lifetime.Singleton);
			builder.Register<IBattlefieldFactory, BattlefieldFactory>(Lifetime.Singleton);
			builder.Register<IArmamentViewFactory, ArmamentViewFactory>(Lifetime.Singleton);
			builder.Register<IStatusFactory, StatusFactory>(Lifetime.Singleton);
			builder.Register<ISystemFactory, SystemFactory>(Lifetime.Singleton);
			builder.Register<IEffectInfoFactory, EffectInfoFactory>(Lifetime.Singleton);
			builder.Register<IStatusesFactory, StatusesFactory>(Lifetime.Singleton);
			builder.Register<IArmamentSpawner, ArmamentSpawner>(Lifetime.Singleton);
			builder.Register<IParticleSpawner, ParticleSpawner>(Lifetime.Singleton);

			return  builder;
		}
	}
}
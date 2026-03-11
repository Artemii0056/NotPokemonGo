using Abilities;
using AbilitiesTestFeature.Services;
using Armaments.Movers;
using Battlefields;
using Castaments;
using CombatText;
using Effects;
using Platoons;
using QteSystem;
using ReactionSystems;
using Services.AssetManagement;
using Services.BattleSessionService;
using Services.BattleUnitContainers;
using Services.Cameras;
using Services.EffectViewServices;
using Services.IdServices;
using Services.LevelProgress;
using Services.RaycastServices;
using Services.SceneServices;
using Services.StaticDataServices;
using Services.UIServices;
using Statuses.Services;
using TimeServices;
using Units;
using Units.AnimationControllers;
using VContainer;

namespace AbilitiesTestFeature.DIExtensions
{
	public static partial class RegisterAbilityTestBattleState
	{
		public static IContainerBuilder RegisterAbilityTestServices(this IContainerBuilder builder)
		{
			builder.Register<IResourceLoader, ResourceLoader>(Lifetime.Singleton);
			builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
			builder.Register<IStaticDataService, StaticDataService>(Lifetime.Singleton);

			builder.Register<IEffectResolver, EffectResolverAbilityTests>(Lifetime.Singleton);
			builder.Register<IBattlefieldProvider, BattlefieldProvider>(Lifetime.Singleton);
			
			builder.Register<IStatusResolver, StatusResolver>(Lifetime.Singleton);
			builder.Register<IStatusManager, StatusManager>(Lifetime.Singleton);
			builder.Register<ICastamentApplicator, CastamentApplicator>(Lifetime.Singleton);
			builder.Register<IRaycastService, RaycastService>(Lifetime.Singleton);
			builder.Register<IQteService, QteService>(Lifetime.Singleton);
			builder.Register<IArmamentMover, ArmamentMover>(Lifetime.Singleton);

			builder.Register<IParticleSystemFactory, ParticleSystemFactory>(Lifetime.Singleton);

			builder.Register<ISourceProvider, SourceProvider>(Lifetime.Singleton);
			builder.Register<IAbilityProvider, AbilityProvider>(Lifetime.Singleton);
			builder.Register<ITargetSelector, TargetSelector>(Lifetime.Singleton);

			builder.Register<ICameraProvider, CameraProvider>(Lifetime.Singleton);

			builder.Register<IBattleUnitContainer, BattleUnitContainer>(Lifetime.Singleton);

			builder.Register<ILevelProgressService, LevelProgressService>(Lifetime.Singleton);

			builder.Register<IBattlefieldSessionService, BattlefieldSessionService>(Lifetime.Singleton);

			builder.Register<IUnitReadyService, UnitReadyService>(Lifetime.Singleton);

			builder.Register<IAbilityService, AbilityServiceAbilityTest>(Lifetime.Singleton);

			builder.Register<ITimeService, TimeService>(Lifetime.Singleton);

			builder.Register<IReactionService, ReactionService>(Lifetime.Singleton);

			builder.Register<UIService>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<IIdService, IdService>(Lifetime.Singleton);

			//builder.Register<CombatTextPresenter>(Lifetime.Singleton);			
			builder.Register<UnitViewRegistry>(Lifetime.Singleton);

			return builder;
		}
	}
}
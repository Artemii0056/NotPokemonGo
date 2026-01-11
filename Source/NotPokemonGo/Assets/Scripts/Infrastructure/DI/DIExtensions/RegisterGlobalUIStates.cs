using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using VContainer;

namespace Infrastructure.DI.DIExtensions
{
	public static partial class ContainerBuilderExtensions
	{
		public static IContainerBuilder RegisterGlobalUIStates(this IContainerBuilder builder)
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
                
			builder.Register<GlobalBattleState>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();
			
			return  builder;
		}
	}
}
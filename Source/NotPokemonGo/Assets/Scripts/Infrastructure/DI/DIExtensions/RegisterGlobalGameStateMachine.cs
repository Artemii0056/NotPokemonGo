using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using VContainer;

namespace Infrastructure.DI.DIExtensions
{
	public static partial class ContainerBuilderExtensions
	{
		public static IContainerBuilder RegisterGameStateMachine(this IContainerBuilder builder)
		{
			builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
			
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
			
			return  builder;
		}
	}
}
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine;
using VContainer;

namespace Infrastructure.DI.DIExtensions
{
	public static partial class ContainerBuilderExtensions
	{
		public static IContainerBuilder RegisterGlobalBattleStateMachine(this IContainerBuilder builder)
		{
			builder.Register<IBattleStateMachine, BattleStateMachine>(Lifetime.Singleton);
			
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
                
			builder.Register<WaveProgressionState>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();
                
			builder.Register<CheckBattleEndState>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();
                
			builder.Register<LoosePanelState>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();
                
			builder.Register<PlayerDodgeState>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();
			
			return  builder;
		}
	}
}
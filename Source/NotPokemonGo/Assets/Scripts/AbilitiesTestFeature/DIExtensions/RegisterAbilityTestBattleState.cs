using AbilitiesTestFeature.BattleStates;
using Infrastructure.StateMachines.BattleStateMachine;
using VContainer;

namespace AbilitiesTestFeature.DIExtensions
{
	public static partial class RegisterAbilityTestBattleState
	{
		public static IContainerBuilder RegisterAbilityTestBattleStates(this IContainerBuilder builder)
		{
			builder.Register<IBattleStateMachine, BattleStateMachine>(Lifetime.Singleton);
			
			builder.Register<CreateBattlfieldAbilityTestState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			builder.Register<EnemyTurnBattleStateAbilityTest>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			builder.Register<PlayerTurnBattleStateAbilityTest>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
	
			return builder;
		}
	}
}
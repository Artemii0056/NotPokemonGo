using UI.Ability;
using UI.BattleUpgrages;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.DIExtensions
{
	public static partial class ContainerBuilderExtensions
	{
		
		public static IContainerBuilder RegisterGlobalUserInterface(
			this IContainerBuilder builder,
			AbilitiesPanel abilitiesPanel,
			BattleUpgradePanel battleUpgradePanel)
		{
			builder.RegisterComponent(abilitiesPanel).AsImplementedInterfaces();
			builder.Register<AbilityPanelPresenter>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();
            
			builder.RegisterComponent(battleUpgradePanel).AsImplementedInterfaces();
			builder.Register<BattleUpgradePanelPresenter>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();
			
			return  builder;
		}
	}
}
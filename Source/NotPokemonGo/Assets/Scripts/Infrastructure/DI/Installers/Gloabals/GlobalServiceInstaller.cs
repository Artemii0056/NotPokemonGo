using System;
using AbilitiesTestFeature.DIExtensions;
using AbilitiesTestFeature.Initializers;
using Infrastructure.DI.DIExtensions;
using Infrastructure.DI.Initializers.Globals;
using Infrastructure.DI.Scopes;
using Services.InputServices;
using UI.Ability;
using UI.BattleUpgrages;
using Units;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Installers.Gloabals
{
	public class GlobalServiceInstaller : MonoInstaller
	{
		[SerializeField] private GameScopeInitializer _gameScopeInitializer;
		[SerializeField] private TestAbilitiesInitializer _testAbilitiesInitializer;
		
		[SerializeField] private InputReader _inputReader;
		[SerializeField] private AbilitiesPanel _abilitiesPanel;
		[SerializeField] private BattleUpgradePanel _battleUpgradePanel;

		[SerializeField] private GameTypeScopeInitializer _gameTypeScopeInitializer;
		
		public override void Install(IContainerBuilder builder)
		{
			builder
				.RegisterGameStateMachine()
				.RegisterGlobalFactories()
				.RegisterGlobalUIStates()
				.RegisterGlobalUserInterface(_abilitiesPanel, _battleUpgradePanel );

			builder.RegisterComponent(_inputReader).AsImplementedInterfaces();

			switch (_gameTypeScopeInitializer)
			{
				case GameTypeScopeInitializer.GameScopeInitializer:
					builder.RegisterComponent(_gameScopeInitializer).AsImplementedInterfaces();
					
					builder
						.RegisterGlobalServices()
						.RegisterGlobalBattleStateMachine()
						;

					_testAbilitiesInitializer.enabled = false;
					break;

				case GameTypeScopeInitializer.TestAbilitiesInitializer:
					builder.RegisterComponent(_testAbilitiesInitializer).AsImplementedInterfaces();
					
					builder
						.RegisterAbilityTestServices()
						.RegisterAbilityTestBattleStates()
						;
					
					_gameScopeInitializer.enabled = false;
					break;
				
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	public enum GameTypeScopeInitializer
	{
		GameScopeInitializer = 1,
		TestAbilitiesInitializer = 2,
	}
}
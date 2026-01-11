using Abilities;
using Armaments;
using Battlefields;
using Cameras;
using Castaments;
using DodgeSystem;
using Effects;
using Infrastructure.DI.DIExtensions;
using Infrastructure.DI.Initializers.Globals;
using Infrastructure.DI.Scopes;
using Infrastructure.StateMachines;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Platoons;
using QTESystem;
using ReactionSystems;
using Services;
using Services.AssetManagement;
using Services.BattleSessionService;
using Services.BattleUnitContainers;
using Services.Cameras;
using Services.InputServices;
using Services.RaycastServices;
using Services.SceneServices;
using Services.StatesServices;
using Services.StaticDataServices;
using Services.SystemFactoryServices;
using Statuses;
using Statuses.Services;
using TimeServices;
using UI.Ability;
using UI.BattleUpgrages;
using UI.Factory;
using UI.QTE;
using Units;
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
            builder
                .RegisterGlobalGameStateMachine()
                .RegisterGlobalServices()
                .RegisterGlobalFactories()
                .RegisterGlobalBattleStateMachine()
                .RegisterGlobalUIStates()
                .RegisterGlobalUserInterface(_abilitiesPanel, _battleUpgradePanel);
            
            builder.RegisterComponent(_gameScopeInitializer).AsImplementedInterfaces();
            builder.RegisterComponent(_inputReader).AsImplementedInterfaces();
        }
    }
}
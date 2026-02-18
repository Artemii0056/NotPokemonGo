using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using RealTimeTickServices;
using Services;
using Services.InputServices;
using Statuses.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Initializers.Globals
{
    public class GameScopeInitializer : MonoBehaviour, IInitializable, ICoroutineRunner
    {
        private IGameStateMachine _gameStateMachine;
        private IInputReader _inputReader;
        private IRealTimeTickService _realTimeTickService;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, IInputReader inputReader, IStatusManager statusManager)
        {
            _gameStateMachine = gameStateMachine;
            _realTimeTickService = statusManager as IRealTimeTickService;
        }

        public void Initialize()
        {
            _gameStateMachine.Enter<BootstrapState>();
        }

        public void Update()
        {
            _gameStateMachine.Update(Time.deltaTime);

            _realTimeTickService.TickRealTime(Time.deltaTime);
        }
    }
}
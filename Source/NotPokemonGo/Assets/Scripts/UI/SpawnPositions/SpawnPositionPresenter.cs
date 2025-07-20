using Characters;
using Infrastructure.MVP.Implementation;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Infrastructure.StateMachines.States;
using UnityEngine;

namespace UI.SpawnPositions
{
    public class SpawnPositionPresenter : IPresenter //TODO Делитать?
    {
        private readonly SpawnPositionView _spawnPositionView;
        private readonly IGameStateMachine _stateMachine;

        public SpawnPositionPresenter(SpawnPositionView spawnPositionView, IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _spawnPositionView = spawnPositionView;
            _spawnPositionView.gameObject.SetActive(false);
        }
        
        public void Enable()
        {
            _spawnPositionView.gameObject.SetActive(true);
            _spawnPositionView.SpawnPositionChanged += OnSpawnPositionChanged;
        }

        public void Disable()
        {
            _spawnPositionView.SpawnPositionChanged -= OnSpawnPositionChanged;
            _spawnPositionView.gameObject.SetActive(false);
        }

        private void OnSpawnPositionChanged(SpawnPositionType spawnPositionType)
        {
            //_stateMachine.Enter<LoadingBattleState, SpawnPositionType>(spawnPositionType);
            Disable();
        }
    }
}
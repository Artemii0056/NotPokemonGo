using Characters;
using Infrastructure.MVP.Implementation;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;

namespace UI.SpawnPositions
{
    public class SpawnPositionPresenter : IPresenter
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
            Debug.Log("OnEnable presenter");
        }

        public void Disable()
        {
            _spawnPositionView.SpawnPositionChanged -= OnSpawnPositionChanged;
            _spawnPositionView.gameObject.SetActive(false);
        }

        private void OnSpawnPositionChanged(SpawnPositionType spawnPositionType)
        {
            _stateMachine.Enter<LoadingBattleState, SpawnPositionType>(spawnPositionType);
            Disable();
        }
    }
}
using Characters;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LocationTypeView : MonoBehaviour
{
    [SerializeField] private SpawnPositionType spawnPositionTypeFirstCommand;
    [SerializeField] private SpawnPositionType spawnPositionTypeSecondCommand;

    [SerializeField] private Button _button;

    [Inject] public IGameStateMachine GameStateMachine;
    
    private void OnEnable() => 
        _button.onClick.AddListener(Test);

    private void OnDisable() => 
        _button.onClick.RemoveListener(Test);

    private void Test()
    {
        var locationTypeInfo = new LocationTypeInfo(spawnPositionTypeFirstCommand, spawnPositionTypeSecondCommand);

        GameStateMachine.Enter<LoadingBattleState, LocationTypeInfo>(locationTypeInfo);
    }
}
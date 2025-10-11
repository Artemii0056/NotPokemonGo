using Infrastructure.StateMachines;
using Infrastructure.StateMachines.BattleStateMachine.States;

namespace Services
{
    public interface ILevelProgressService
    {
        public LevelRuntimeDataPayload LevelData { get; }
        void Set(LevelRuntimeDataPayload levelData);
    }
}
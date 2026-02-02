using Infrastructure.StateMachines.BattleStateMachine.States;

namespace Services.LevelProgress
{
    public interface ILevelProgressService
    {
        public LevelRuntimeDataPayload LevelData { get; }
        void Set(LevelRuntimeDataPayload levelData);
    }
}
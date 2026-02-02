using Infrastructure.StateMachines.BattleStateMachine.States;

namespace Services.LevelProgress
{
    public class LevelProgressService : ILevelProgressService //TODO А вот это наверное не нужно
    {
        public LevelRuntimeDataPayload LevelData { get; private set; }

        public void Set(LevelRuntimeDataPayload levelData) => 
            LevelData = levelData;
    }
}
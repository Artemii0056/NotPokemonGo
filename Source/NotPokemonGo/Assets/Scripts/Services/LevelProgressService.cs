using Infrastructure.StateMachines;

namespace Services
{
    public class LevelProgressService : ILevelProgressService
    {
        public LevelRuntimeDataPayload LevelData { get; private set; }

        public void Set(LevelRuntimeDataPayload levelData) => 
            LevelData = levelData;
    }
}
using Infrastructure.StateMachines;

namespace Services
{
    public class LevelProgressService : ILevelProgressService //TODO А вот это наверное не нужно
    {
        public LevelRuntimeDataPayload LevelData { get; private set; }

        public void Set(LevelRuntimeDataPayload levelData) => 
            LevelData = levelData;
    }
}
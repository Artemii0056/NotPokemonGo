using Infrastructure.StateMachines;

namespace Services
{
    public interface ILevelProgressService
    {
        public LevelRuntimeDataPayload LevelData { get; }
        void Set(LevelRuntimeDataPayload levelData);
    }
}
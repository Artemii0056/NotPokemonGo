using Infrastructure.StateMachines;
using LevelSetting;
using Platoons;

namespace Services.BattleSessionService
{
    public interface IBattlefieldSessionService
    {
        Battlefield StartNewBattle(LevelRuntimeDataPayload levelData, LevelPartSetup levelPartSetup, Platoon survivors = null);
        void Cleanup();
    }
}
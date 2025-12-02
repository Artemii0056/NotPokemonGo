using Battlefields;
using Infrastructure.StateMachines;
using Infrastructure.StateMachines.BattleStateMachine.States;
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
using UI;

namespace Infrastructure.StateMachines.BattleStateMachine.Payloads
{
    public class StartMenuPayload
    {
        public readonly StartScreenUI UI;

        public StartMenuPayload(StartScreenUI ui) => 
            UI = ui;
    }
}
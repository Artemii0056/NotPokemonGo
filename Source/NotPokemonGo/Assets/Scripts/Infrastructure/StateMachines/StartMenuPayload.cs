using UI;

namespace Infrastructure.StateMachines
{
    public class StartMenuPayload
    {
        public readonly StartScreenUI UI;

        public StartMenuPayload(StartScreenUI ui) => 
            UI = ui;
    }
}
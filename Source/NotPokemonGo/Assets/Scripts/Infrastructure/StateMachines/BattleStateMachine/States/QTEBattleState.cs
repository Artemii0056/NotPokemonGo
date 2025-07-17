using Infrastructure.StateMachines.States.Interfaces;
using Services.QTEServices;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class QTEBattleState : IPayloadedState<Battlefield>
    {
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IQTEService _qteService;

        public QTEBattleState(IBattleStateMachine battleStateMachine, IQTEService qteService)
        {
            _battleStateMachine = battleStateMachine;
            _qteService = qteService;
        }
        
        public void Enter(Battlefield unitActionPayload)
        {
            _qteService.Start(unitActionPayload);
        }

        public void Exit()
        {
            
        }
    }
}
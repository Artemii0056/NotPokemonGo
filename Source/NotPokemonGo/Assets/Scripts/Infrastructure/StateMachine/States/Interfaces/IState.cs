namespace Infrastructure.StateMachine.States.Interfaces
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}
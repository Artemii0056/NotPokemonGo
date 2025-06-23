namespace Infrastructure.StateMachines.States.Interfaces
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}
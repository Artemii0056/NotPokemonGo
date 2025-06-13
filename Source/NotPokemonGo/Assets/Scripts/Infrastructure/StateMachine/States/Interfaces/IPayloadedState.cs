namespace Infrastructure.StateMachine.States.Interfaces
{
    public interface IPayloadedState<IPayload>: IExitableState
    {
        void Enter(IPayload payload);
    }
}
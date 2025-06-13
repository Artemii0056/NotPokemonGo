namespace Infrastructure.StateMachine
{
    public interface IPayloadedState<IPayload>: IExitableState
    {
        void Enter(IPayload payload);
    }
}
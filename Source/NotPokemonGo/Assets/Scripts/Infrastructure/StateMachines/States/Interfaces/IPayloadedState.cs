namespace Infrastructure.StateMachines.States.Interfaces
{
    public interface IPayloadedState<IPayload>: IExitableState
    {
        void Enter(IPayload unitActionPayload);
    }
}
public interface IState : IExitableState
{
    void Enter();
}

public interface IPayloadedState<IPayload>: IExitableState
{
    void Enter(IPayload payload);
}

public interface IExitableState
{
    void Exit();
}   
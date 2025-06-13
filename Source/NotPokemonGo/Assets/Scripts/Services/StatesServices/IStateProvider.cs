namespace Infrastructure.StateMachine
{
    public interface IStateProvider
    {
        T GetState <T>() where T : IExitableState;
    }
}
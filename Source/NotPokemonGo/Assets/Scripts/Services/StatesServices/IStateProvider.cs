using Infrastructure.StateMachines.States.Interfaces;

namespace Services.StatesServices
{
    public interface IStateProvider
    {
        T GetState <T>() where T : IExitableState;
    }
}
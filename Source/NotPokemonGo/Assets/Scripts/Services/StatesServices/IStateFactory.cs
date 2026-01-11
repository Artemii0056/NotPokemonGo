using Infrastructure.StateMachines.States.Interfaces;

namespace Services.StatesServices
{
    public interface IStateFactory
    {
        T GetState <T>() where T : IExitableState;
    }
}
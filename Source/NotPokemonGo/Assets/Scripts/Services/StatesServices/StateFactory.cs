using Infrastructure.StateMachines.States.Interfaces;
using VContainer;

namespace Services.StatesServices
{
    public class StateFactory : IStateFactory
    {
        private readonly IObjectResolver _container;

        public StateFactory(IObjectResolver  container) => 
            _container = container;
        
        public T GetState <T>() where T : IExitableState => 
            _container.Resolve<T>();
    }
}
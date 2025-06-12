using VContainer;

namespace Infrastructure.StateMachine
{
    public class StateProvider : IStateProvider
    {
        private readonly IObjectResolver _container;

        public StateProvider(IObjectResolver  container) => 
            _container = container;
        
        public T GetState <T>() where T : IExitableState => 
            _container.Resolve<T>();
    }
}
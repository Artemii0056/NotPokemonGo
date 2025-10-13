using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services.SystemFactoryServices
{
    public class SystemFactory : ISystemFactory
    {
        private readonly IObjectResolver _container;

        public SystemFactory(IObjectResolver  container) => 
            _container = container;

        public T Create<T>(T prefab) where T : MonoBehaviour => 
            _container.Instantiate(prefab);
    }
}
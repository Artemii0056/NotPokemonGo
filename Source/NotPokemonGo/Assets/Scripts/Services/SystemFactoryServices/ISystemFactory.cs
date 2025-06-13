using UnityEngine;

namespace Services.SystemFactoryServices
{
    public interface ISystemFactory
    {
        T Create<T>(T prefab) where T : MonoBehaviour;
    }
}
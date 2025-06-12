using UnityEngine;

namespace Infrastructure.Scripts.AssetManagement
{
    public interface ISystemFactory
    {
        T Create<T>(T prefab) where T : MonoBehaviour;
    }
}
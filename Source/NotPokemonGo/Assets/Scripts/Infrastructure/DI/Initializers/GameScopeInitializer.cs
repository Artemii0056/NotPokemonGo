using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Initializers
{
    public class GameScopeInitializer : MonoBehaviour, IInitializable
    {
        public void Initialize()
        {
            Debug.Log("ТОЧКА ВХОДА GameScopeInitializer.Initialize");
        }
    }
}
using UnityEngine;

namespace Services.AssetManagement
{
    public interface IResourceLoader
    {
        T Load<T>(string path) where T : Object;
        T LoadScriptableObject<T>(string path) where T : ScriptableObject;
    }
}
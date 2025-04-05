using UnityEngine;

namespace Infrastructure.Scripts.AssetManagement
{
    public interface IResourceLoader
    {
        GameObject Instantiate(string path);
        T Instantiate<T>(string path) where T : Component;
        T Load<T>(string path) where T : Object;
        T LoadScriptableObject<T>(string path) where T : ScriptableObject;
    }
}
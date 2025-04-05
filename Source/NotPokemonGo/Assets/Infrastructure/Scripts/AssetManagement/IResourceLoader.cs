using UnityEngine;

namespace Infrastructure.Scripts.AssetManagement
{
    public interface IResourceLoader
    {
        GameObject Instantiate(string path);
        T LoadScriptableObject<T>(string path) where T : ScriptableObject;
    }
}
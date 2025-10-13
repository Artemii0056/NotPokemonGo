using UnityEngine;

namespace Services.AssetManagement
{
    public class ResourceLoader : IResourceLoader 
    {
        public T Load<T>(string path) where T : Object => 
            Resources.Load<T>(path);

        public T LoadScriptableObject<T>(string path) where T : ScriptableObject => 
            Resources.Load<T>(path);
    }
}
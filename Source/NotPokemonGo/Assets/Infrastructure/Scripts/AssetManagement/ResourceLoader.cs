using UnityEngine;

namespace Infrastructure.Scripts.AssetManagement
{
    public class ResourceLoader : IResourceLoader 
    {
        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);

            return Object.Instantiate(prefab);
        }
        
        public T Instantiate<T>(string path) where T : Component
        {
            GameObject prefab = Load<GameObject>(path);
            GameObject instance = Object.Instantiate(prefab);
            return instance.GetComponent<T>();
        }
        
        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public T LoadScriptableObject<T>(string path) where T : ScriptableObject => 
            Resources.Load<T>(path);
    }
}
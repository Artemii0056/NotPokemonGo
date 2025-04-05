using UnityEngine;

namespace Infrastructure.Scripts.AssetManagement
{
    public class ResourceLoader : IResourceLoader 
    {
        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);

            if (prefab == null)
            {
                Debug.Log(path);
            }

            return Object.Instantiate(prefab);
        }

        public T LoadScriptableObject<T>(string path) where T : ScriptableObject => 
            Resources.Load<T>(path);

        // public T Instantiate<T>(string path)
        // {
        //    // T obj = Resources.Load<T>(path);
        //    // T obj = Instantiate<T>(path);
        //     
        //     return obj;
        // }
    }
}
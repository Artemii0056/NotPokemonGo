using System;
using Services.AssetManagement;
using Services.ObjectPools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Services.Spawners
{
  public abstract class BaseSpawner <T> where T : MonoBehaviour, IPoolabelObject
  {
    protected readonly IObjectPool<T> ObjectPool;

    protected readonly IObjectResolver ObjectResolver;
    protected readonly IResourceLoader ResourceLoader;

    protected BaseSpawner(IObjectResolver objectResolver, IResourceLoader resourceLoader)
    {
      ObjectResolver = objectResolver;
      ResourceLoader = resourceLoader;

      ObjectPool = new ObjectPool<T>(
        CreateObject,
        OnGetFromPool,
        ReleaseObject,
        false);
    }
    
    protected abstract T GetPrefab();

    protected virtual void OnGetFromPool(T poolableObject)
    {
      poolableObject.gameObject.SetActive(true);
    }

    protected virtual void ReleaseObject(T poolabelObject)
    {
      poolabelObject.PollableDispose();
      poolabelObject.gameObject.SetActive(false);
    }

    private T CreateObject()
    {
      T prefab = GetPrefab();

      if (prefab == null)
        throw new InvalidOperationException(
          $"[{GetType().Name}] Prefab is null in GetPrefab()");

      return ObjectResolver.Instantiate(prefab);
    }
  }
}
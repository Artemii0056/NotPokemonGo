using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Spawners
{
	public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IPooledObject<T>
	{
		[Header("Spawner settings")]
		[SerializeField] private T _prefab;
		[SerializeField] private Transform _container;

		private IObjectPool<T> _pool;

		public event Action DataChanged;

		public int InstancesCount { get; private set; }
		public int SpawnedEverCount { get; private set; }
		public int ActiveCount { get; private set; }

		private void Awake()
			=> _pool = new ObjectPool<T>(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy);

		protected T Spawn(Vector3 position)
		{
			T spawnable = Spawn();
			spawnable.transform.position = position;

			return spawnable;
		}

		protected T Spawn()
		{
			T spawnable = _pool.Get();
			spawnable.Destroyed += OnPoolObjectDestroyed;

			SpawnedEverCount++;
			DataChanged?.Invoke();

			return spawnable;
		}

		protected virtual void OnPoolObjectDestroyed(T pooledObject)
		{
			pooledObject.Destroyed -= OnPoolObjectDestroyed;
			_pool.Release(pooledObject);
		}

		private T OnPoolCreate()
		{
			T spawnable = Instantiate(_prefab, _container);
			InstancesCount++;

			DataChanged?.Invoke();

			return spawnable;
		}

		private void OnPoolGet(T pooledObject)
		{
			pooledObject.gameObject.SetActive(true);
			ActiveCount++;

			DataChanged?.Invoke();
		}

		private void OnPoolRelease(T pooledObject)
		{
			pooledObject.gameObject.SetActive(false);
			ActiveCount--;
        
			DataChanged?.Invoke();
		}

		private void OnPoolDestroy(T pooledObject)
			=> Destroy(pooledObject.gameObject);
	}
}
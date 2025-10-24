using System;
using System.Collections.Generic;

namespace Services.ObjectPools
{
  public class ObjectPool <T> : IObjectPool<T> where T : IPoolabelObject
  {
    private readonly Func<T> _onCreate;
    private readonly Action _onGet;
    private readonly Action _onRelease;
    private readonly Queue<T> _pool = new Queue<T>();

    public ObjectPool(
      Func<T> onCreate,
      Action onGet,
      Action onRelease,
      bool isIncreasing = false, 
      int defaultLength = 10)
    {
      _onCreate = onCreate;
      _onGet = onGet;
      _onRelease = onRelease;
    }
    
    public IEnumerable<T> Items => _pool;
    
    public void Release(T item)
    {
      _onRelease?.Invoke();
      _pool.Enqueue(item);
    }

    public T Get()
    {
      if (_pool.Count == 0) 
        _pool.Enqueue(_onCreate.Invoke());
      
      _onGet?.Invoke();
      return _pool.Dequeue();
    } 
  }
}
using System;
using System.Collections.Generic;

namespace Services.ObjectPools
{
  public class ObjectPool <T> : IObjectPool<T> where T : IPoolabelObject
  {
    private readonly Func<T> _onCreate;
    private readonly Action<T> _onGet;
    private readonly Action<T> _onRelease;
    private readonly Queue<T> _pool = new();
    private readonly bool _isIncreasing;
    private readonly int _defaultLength;

    public ObjectPool(
      Func<T> onCreate,
      Action<T> onGet,
      Action<T> onRelease,
      bool isIncreasing = false,
      int defaultLength = 10)
    {
      _onCreate = onCreate ?? throw new ArgumentNullException(nameof(onCreate));
      _onGet = onGet;
      _onRelease = onRelease;
      _isIncreasing = isIncreasing;
      _defaultLength = Math.Max(0, defaultLength);
    }

    public IEnumerable<T> Items => _pool;
    
    public void Release(T item)
    {
      _onRelease?.Invoke(item);
      _pool.Enqueue(item);
    }

    public T Get()
    {
      if (_pool.Count == 0)
      {
        _pool.Enqueue(_onCreate());
      }

      T item = _pool.Dequeue();
      _onGet?.Invoke(item);
      return item;
    }
  }
}
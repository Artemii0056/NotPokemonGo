using System.Collections.Generic;

namespace Services.ObjectPools
{
  public interface IObjectPool<T> where T : IPoolabelObject
  {
    IEnumerable<T> Items { get; }
    void Release(T item);
    T Get();
  }
}
using System;

namespace Spawners
{
    public interface IPooledObject<T>
    {
        event Action<T> Destroyed;
    }
}

using System;

namespace InputServices
{
    public interface IRaycastService<T>
    {
        event Action<T> Raycasted;
        event Action NotCollided;
        event Action NotFinded;
    }
}
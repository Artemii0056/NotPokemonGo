using System;
using Units;

namespace InputServices
{
    public interface IRaycaster
    {
        event Action<Unit> UnitSearched;
        void OnLeftMouseButtonPressed();
    }
}
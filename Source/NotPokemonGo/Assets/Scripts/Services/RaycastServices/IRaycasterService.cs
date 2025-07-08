using System;
using Units;

namespace InputServices
{
    public interface IRaycasterService
    {
        event Action<Unit> UnitSearched;
        void OnLeftMouseButtonPressed();
    }
}
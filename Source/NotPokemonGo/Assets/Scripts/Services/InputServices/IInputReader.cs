using System;

namespace Services.InputServices
{
    public interface IInputReader
    {
        event Action LeftMouseButtonPressed;
        event Action SpacePressed;
        event Action EButtonPressed;
    }
}
using System;

namespace InputServices
{
    public interface IInputReader
    {
        event Action LeftMouseButtonPressed;
    }
}
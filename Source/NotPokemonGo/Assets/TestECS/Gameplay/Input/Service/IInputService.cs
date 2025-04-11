using UnityEngine;

namespace TestECS.Gameplay.Input
{
    public interface IInputService
    {
        float GetVerticalAxis();
        float GetHorizontalAxis();
        bool HasAxisInput();
    
        bool GetLeftMouseButtonDown();
        Vector2 GetScreenMousePosition();
        Vector2 GetWorldMousePosition();
        bool GetLeftMouseButtonUp();
    }
}
using UnityEngine;

namespace Services.RaycastServices
{
    public interface IRaycastService
    {
        bool Raycast<T>(out T component) where T : MonoBehaviour;
    }
}
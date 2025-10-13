using UnityEngine;

namespace Services.RaycastServices
{
  public class RaycastService : IRaycastService
  {
    public bool Raycast<T>(out T component) where T : MonoBehaviour
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      component = null;
      
      if (Physics.Raycast(ray, out RaycastHit hit))
        if (hit.collider.TryGetComponent(out component))
          return true;

      return false;
    }
  }
}
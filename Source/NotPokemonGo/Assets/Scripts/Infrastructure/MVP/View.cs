using Infrastructure.MVP.Implementation;
using UnityEngine;

namespace Infrastructure.MVP
{
    public class View : MonoBehaviour, IView
    {
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
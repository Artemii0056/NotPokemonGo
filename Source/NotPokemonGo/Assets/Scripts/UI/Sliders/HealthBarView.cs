using UnityEngine;

namespace UI.Sliders
{
    public class HealthBarView: MonoBehaviour
    {
        [SerializeField] private StatSliderView _bar;

        public void Set(float current, float max)
        {
            _bar.ChangeFilling(current, max);
        }
    }
}

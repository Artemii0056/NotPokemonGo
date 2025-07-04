using UnityEngine;
using UnityEngine.UI;

namespace UI.Sliders
{
    public class StatSliderView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        
        public void ChangeFilling(float currentValue, float maxValue)
        {
            float ratio = Mathf.Clamp01((currentValue / maxValue));
            _icon.fillAmount = ratio;
        }
    }
}
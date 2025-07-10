using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UnitSelectionPanel : MonoBehaviour
    {
        [SerializeField] private Image _unitIcon;
        [SerializeField] private Image _background;

        public void SetIcon(Sprite icon)
        {
            _unitIcon.sprite = icon;
        }

        public void SetBackground(Sprite background)
        {
            _background.sprite = background;
        }
    }
}